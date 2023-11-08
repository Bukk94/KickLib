using System.Text.RegularExpressions;
using KickLib.Exceptions;
using KickLib.Interfaces;
using KickLib.Models;
using Newtonsoft.Json.Linq;
using Polly;
using PuppeteerSharp;

namespace KickLib.Clients;

/// <summary>
///     Browser-like client that simulates user visiting the website.
///     This client is slower but bypasses most of the Kick's API protections.
/// </summary>
public class BrowserClient : IApiCaller
{
    private readonly Regex _regex = new(@"<body>(?<json>.+)<\/body>", RegexOptions.Compiled);
    private readonly IAuthenticationService _authenticationService;
    private readonly BrowserSettings _settings;
    
    public BrowserClient(
        IAuthenticationService authenticationService,
        BrowserSettings settings)
    {
        _authenticationService = authenticationService;
        _settings = settings ?? BrowserSettings.Empty;
    }
    
    public Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
    {
        return _authenticationService.AuthenticateAsync(authenticationSettings);
    }
    
    public async Task<KeyValuePair<int, string>> SendRequestAsync(string url)
    {
        await using var browser = await BrowserInitializer.LaunchBrowserAsync(_settings);
        
        await using var page = await browser.NewPageAsync();
        await page.GoToAsync(url);
        await page.WaitForSelectorAsync("body:not([class])");
        
        var content = await page.GetContentAsync();

        var match = _regex.Match(content);
        
        return new KeyValuePair<int, string>(
            match.Success ? 200 : 500,
            match.Success ? match.Groups["json"].Value : string.Empty);
    }

    public async Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(string url, string payload)
    {
        if (!_authenticationService.IsAuthenticated)
        {
            throw new ArgumentException($"Cannot send authenticated request without authenticating first! Call '{nameof(AuthenticateAsync)}' first.");
        }
        
        await using var browser = await BrowserInitializer.LaunchBrowserAsync(_settings);
        
        try
        {
            await using var page = await browser.NewPageAsync();
            
            var method = payload is not null ? "POST" : "GET";
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
                    response = await GetApiResponseAsync(page, url, method, body);
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
            Console.WriteLine($"Error sending message: {ex.Message}");
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
                ");

                if (response.Contains("CSRF token mismatch"))
                {
                    throw new XsrfMismatchException("Something went wrong: CSRF token mismatch");
                }

                return response;
            });
    }
}