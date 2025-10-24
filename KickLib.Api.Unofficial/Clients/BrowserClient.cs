using KickLib.Api.Unofficial.Clients.Puppeteer;
using KickLib.Api.Unofficial.Exceptions;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using KickLib.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Polly;
using PuppeteerSharp;
using System.Text.RegularExpressions;
using Polly.Retry;

namespace KickLib.Api.Unofficial.Clients
{
    /// <summary>
    ///     Browser-like client that simulates user visiting the website.
    ///     This client is slower but bypasses most of the Kick's API protections.
    /// </summary>
    public class BrowserClient : IApiCaller
    {
        private readonly Regex _regex = new(@"<body>(?<json>.+)<\/body>", RegexOptions.Compiled);
        private readonly IAuthenticationService _authenticationService;
        private readonly BrowserSettings _settings;
        private readonly ILogger _logger;
        private readonly AsyncRetryPolicy<KeyValuePair<int, string>> _retryPolicy;

        public BrowserClient(
            IAuthenticationService authenticationService,
            BrowserSettings settings,
            ILogger logger = null)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _settings = settings ?? BrowserSettings.Empty;
            
            // Polly retry policy with delay between each attempt
            _retryPolicy = Policy
                .Handle<Exception>() // Handle all exceptions
                .OrResult<KeyValuePair<int, string>>(x => x.Key >= 500) // Handle 500 error responses
                .WaitAndRetryAsync(
                    retryCount: _settings.MaxRetryCount,
                    sleepDurationProvider: _ => _settings.RetryDelay,
                    onRetry: (_, _, retryCount, context) =>
                    {
                        context.TryGetValue("url", out var url);
                        // Log retry information for debugging
                        _logger.LogInformation("Retry attempt {RetryCount} for URL: {Value}", retryCount, url);
                    });
        }

        /// <inheritdoc />
        public Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            return _authenticationService.AuthenticateAsync(authenticationSettings);
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<int, string>> SendRequestAsync(string url)
        {
            return await _retryPolicy.ExecuteAsync(async (_) =>
            {
                await using var browser = await BrowserInitializer.LaunchBrowserAsync(_settings).ConfigureAwait(false);

                await using var page = await browser.NewPageAsync().ConfigureAwait(false);
                await page.GoToAsync(url);

                try
                {
                    // Wait for one of following selector:
                    //    1) <body> with no class - that means JSON response with no formatting -> expected behavior
                    //    2) Specific div that contains error name -> this happens when Kick throw 404 or 500 errors (we catch this to avoid timeout waiting)
                    await page.WaitForSelectorAsync(
                        "body:not([class]), body > div > div > div > div.ml-4.text-lg.text-gray-500.uppercase.tracking-wider");
                }
                catch (Exception ex)
                {
                    throw new KickLibException("KickLib failed to get response from Kick.com. See inner exception for details.", ex);
                }

                var content = await page.GetContentAsync().ConfigureAwait(false);
                var match = _regex.Match(content);

                if (!match.Success)
                {
                    return GetErrorResponse(content);
                }

                return new KeyValuePair<int, string>(200, match.Groups["json"].Value);
            }, new Dictionary<string, object> { { "url", url } }).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(
            string url, 
            string payload,
            HttpMethod? method = null)
        {
            if (!_authenticationService.IsAuthenticated)
            {
                throw new ArgumentException($"Cannot send authenticated request without authenticating first! Call '{nameof(AuthenticateAsync)}' first.");
            }

            await using var browser = await BrowserInitializer.LaunchBrowserAsync(_settings).ConfigureAwait(false);

            try
            {
                await using var page = await browser.NewPageAsync().ConfigureAwait(false);

                var requestMethod = method?.ToString() ?? (payload is not null ? "POST" : "GET");
                var body = payload is not null
                    ? $", body: JSON.stringify({payload})"
                    : "";

                string response = null;
                await Policy
                    .Handle<XsrfMismatchException>()
                    .RetryAsync(2, async (_, _) =>
                    {
                        // Refresh Xsrf token if we get a mismatch
                        await _authenticationService.RefreshXsrfTokenAsync(page);
                    })
                    .ExecuteAsync(async () =>
                    {
                        response = await GetApiResponseAsync(page, url, requestMethod, body).ConfigureAwait(false);
                    });

                if (response is null)
                {
                    throw new ArgumentException("Couldn't get the response from target page");
                }

                var parsedResponse = JToken.Parse(response);
                if (parsedResponse["message"] != null)
                {
                    // if root contains 'message' it's most likely error
                    return new KeyValuePair<int, string>(500, parsedResponse["message"].ToString());
                }

                // Try to extract status code from the call. It's not always there.
                var statusCode = int.Parse(parsedResponse["status"]?["code"]?.ToString() ?? "200");

                return new KeyValuePair<int, string>(statusCode, response);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error sending message: {ex.Message}");
            }

            // At this point we got error, so return empty string with 500.
            return new KeyValuePair<int, string>(500, string.Empty);
        }

        private async Task<string> GetApiResponseAsync(IPage page, string url, string method, string body)
        {
            return await Policy
                .Handle<PuppeteerException>()
                .RetryAsync(3)
                .ExecuteAsync(async () =>
                {
                    // Sometimes Kick doesn't like our requests and we get 'Failed to fetch' exception
                    // Simple retry is enough to pass through 

                    var response = await page.EvaluateFunctionAsync<string>($@"
                    async () => {{
                        const response = await fetch('{url}', {{
                            method: '{method}',
                            headers: {{
                                'Accept': 'application/json',
                                'Content-Type': 'application/json',
                                'Authorization': 'Bearer {_authenticationService.BearerToken}',
                                'X-Xsrf-Token': '{_authenticationService.XsrfToken}'
                            }}{body}
                        }});
                        return response.text();
                    }}
                ").ConfigureAwait(false);

                    if (response.Contains("CSRF token mismatch"))
                    {
                        throw new XsrfMismatchException("Something went wrong: CSRF token mismatch");
                    }

                    return response;
                });
        }

        private static KeyValuePair<int, string> GetErrorResponse(string pageContent)
        {
            if (pageContent.Contains("<title>Server Error</title>"))
            {
                // Kick throws 500
                return new KeyValuePair<int, string>(503, string.Empty);
            }

            if (pageContent.Contains("<title>Not Found</title>"))
            {
                // Kick sends Not found error
                return new KeyValuePair<int, string>(404, string.Empty);
            }

            // In all other cases, it's probably library fault
            return new KeyValuePair<int, string>(500, string.Empty);
        }
    }
}