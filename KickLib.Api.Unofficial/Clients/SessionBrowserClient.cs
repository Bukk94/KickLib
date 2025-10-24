using System.Text.RegularExpressions;
using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Exceptions;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;
using Polly;

namespace KickLib.Api.Unofficial.Clients
{
    /// <summary>
    ///     Optimized session-based browser client that uses shared browser instance and supports multiple users
    /// </summary>
    public class SessionBrowserClient : IApiCaller
    {
        private readonly Regex _regex = new(@"<body>(?<json>.+)<\/body>", RegexOptions.Compiled);
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger _logger;
        private readonly string _sessionId;
        private readonly SessionManager _sessionManager;
        private readonly BrowserManager _browserManager;

        public SessionBrowserClient(
            IAuthenticationService authenticationService,
            string sessionId,
            BrowserManager browserManager,
            SessionManager sessionManager,
            ILogger logger = null)
        {
            _authenticationService = authenticationService;
            _logger = logger;
            _sessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _browserManager = browserManager ?? throw new ArgumentNullException(nameof(browserManager));
        }

        /// <inheritdoc />
        public Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            return _authenticationService.AuthenticateAsync(authenticationSettings);
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<int, string>> SendRequestAsync(string url)
        {
            var session = _sessionManager.GetSession(_sessionId);
            if (session == null)
            {
                throw new InvalidOperationException($"Session {_sessionId} not found");
            }

            try
            {
                // Use fetch API instead of page navigation for better performance
                var response = await _browserManager.ExecuteFetchRequestAsync(
                    _sessionId,
                    url,
                    "GET",
                    null,
                    new Dictionary<string, string>
                    {
                        ["Accept"] = "application/json",
                        ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                        ["Authorization"] = !string.IsNullOrEmpty(session.BearerToken) ? $"Bearer {session.BearerToken}" : null,
                        ["X-Xsrf-Token"] = session.XsrfToken
                    }.Where(h => h.Value != null).ToDictionary(h => h.Key, h => h.Value)
                );

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
                var statusCode = (int)result.status;
                var body = result.body?.ToString() ?? "";

                if (statusCode == 200)
                {
                    // Try to extract JSON from response body
                    var match = _regex.Match(body);
                    if (match.Success)
                    {
                        body = match.Groups["json"].Value;
                    }

                    return new KeyValuePair<int, string>(statusCode, body);
                }
                else if (statusCode == 419) // CSRF token mismatch
                {
                    // Try to refresh XSRF token and retry
                    _logger?.LogWarning("CSRF token mismatch for session {SessionId}, attempting to refresh token", _sessionId);
                    
                    var page = await _browserManager.GetOrCreateSessionPageAsync(_sessionId);
                    await _authenticationService.RefreshXsrfTokenAsync(page);
                    
                    // Retry the request with new token
                    session = _sessionManager.GetSession(_sessionId);
                    var retryResponse = await _browserManager.ExecuteFetchRequestAsync(
                        _sessionId,
                        url,
                        "GET",
                        null,
                        new Dictionary<string, string>
                        {
                            ["Accept"] = "application/json",
                            ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                            ["Authorization"] = !string.IsNullOrEmpty(session.BearerToken) ? $"Bearer {session.BearerToken}" : null,
                            ["X-Xsrf-Token"] = session.XsrfToken
                        }.Where(h => h.Value != null).ToDictionary(h => h.Key, h => h.Value)
                    );

                    var retryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(retryResponse);
                    var retryStatusCode = (int)retryResult.status;
                    var retryBody = retryResult.body?.ToString() ?? "";

                    if (retryStatusCode == 200)
                    {
                        var retryMatch = _regex.Match(retryBody);
                        if (retryMatch.Success)
                        {
                            retryBody = retryMatch.Groups["json"].Value;
                        }
                    }

                    return new KeyValuePair<int, string>(retryStatusCode, retryBody);
                }
                else
                {
                    return new KeyValuePair<int, string>(statusCode, body);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error sending request to {Url} for session {SessionId}", url, _sessionId);
                return GetErrorResponse(ex.Message);
            }
        }

        /// <inheritdoc />
        public async Task<KeyValuePair<int, string>> SendAuthenticatedRequestAsync(string url, string payload, HttpMethod? method = null)
        {
            var session = _sessionManager.GetSession(_sessionId);
            method ??= HttpMethod.Post;
            if (session == null)
            {
                throw new InvalidOperationException($"Session {_sessionId} not found");
            }

            if (!session.IsAuthenticated)
            {
                throw new UnauthorizedAccessException("Session is not authenticated");
            }

            try
            {
                var headers = new Dictionary<string, string>
                {
                    ["Accept"] = "application/json",
                    ["Content-Type"] = "application/json",
                    ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36",
                    ["Authorization"] = $"Bearer {session.BearerToken}",
                    ["X-Xsrf-Token"] = session.XsrfToken
                };

                var response = await _browserManager.ExecuteFetchRequestAsync(
                    _sessionId,
                    url,
                    method.ToString(),
                    payload,
                    headers
                );

                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
                var statusCode = (int)result.status;
                var body = result.body?.ToString() ?? "";

                if (statusCode == 419) // CSRF token mismatch
                {
                    _logger?.LogWarning("CSRF token mismatch for session {SessionId}, attempting to refresh token", _sessionId);
                    
                    var page = await _browserManager.GetOrCreateSessionPageAsync(_sessionId);
                    await _authenticationService.RefreshXsrfTokenAsync(page);
                    
                    // Retry with new token
                    session = _sessionManager.GetSession(_sessionId);
                    headers["X-Xsrf-Token"] = session.XsrfToken;
                    
                    var retryResponse = await _browserManager.ExecuteFetchRequestAsync(
                        _sessionId,
                        url,
                        method.ToString(),
                        payload,
                        headers
                    );

                    var retryResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(retryResponse);
                    statusCode = (int)retryResult.status;
                    body = retryResult.body?.ToString() ?? "";
                }

                return new KeyValuePair<int, string>(statusCode, body);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error sending authenticated request to {Url} for session {SessionId}", url, _sessionId);
                return GetErrorResponse(ex.Message);
            }
        }

        public async Task<string> SendPostRequestAsync(string url, object jsonPayload, bool useAuthentication = true)
        {
            var session = _sessionManager.GetSession(_sessionId);
            if (session == null)
            {
                throw new InvalidOperationException($"Session {_sessionId} not found");
            }

            var headers = new Dictionary<string, string>
            {
                ["Accept"] = "application/json",
                ["Content-Type"] = "application/json",
                ["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36"
            };

            if (useAuthentication)
            {
                if (!string.IsNullOrEmpty(session.BearerToken))
                {
                    headers["Authorization"] = $"Bearer {session.BearerToken}";
                }
                if (!string.IsNullOrEmpty(session.XsrfToken))
                {
                    headers["X-Xsrf-Token"] = session.XsrfToken;
                }
            }

            return await Policy
                .Handle<XsrfMismatchException>()
                .RetryAsync(1, async (exception, retryCount) =>
                {
                    _logger?.LogWarning("CSRF token mismatch for session {SessionId}, refreshing token (attempt {RetryCount})", _sessionId, retryCount);
                    
                    var page = await _browserManager.GetOrCreateSessionPageAsync(_sessionId);
                    await _authenticationService.RefreshXsrfTokenAsync(page);
                })
                .ExecuteAsync(async () =>
                {
                    var currentSession = _sessionManager.GetSession(_sessionId);
                    if (useAuthentication && !string.IsNullOrEmpty(currentSession.XsrfToken))
                    {
                        headers["X-Xsrf-Token"] = currentSession.XsrfToken;
                    }

                    var jsonBody = Newtonsoft.Json.JsonConvert.SerializeObject(jsonPayload);
                    var response = await _browserManager.ExecuteFetchRequestAsync(
                        _sessionId,
                        url,
                        "POST",
                        jsonBody,
                        headers
                    );

                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response);
                    var body = result.body?.ToString() ?? "";

                    if (body.Contains("CSRF token mismatch"))
                    {
                        throw new XsrfMismatchException("Something went wrong: CSRF token mismatch");
                    }

                    return body;
                });
        }

        private static KeyValuePair<int, string> GetErrorResponse(string errorMessage)
        {
            if (errorMessage.Contains("Server Error"))
            {
                return new KeyValuePair<int, string>(503, string.Empty);
            }

            if (errorMessage.Contains("Not Found"))
            {
                return new KeyValuePair<int, string>(404, string.Empty);
            }

            return new KeyValuePair<int, string>(500, string.Empty);
        }
    }
}
