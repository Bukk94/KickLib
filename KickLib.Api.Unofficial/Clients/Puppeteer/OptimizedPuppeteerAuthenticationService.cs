using System.Dynamic;
using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OtpNet;
using PuppeteerSharp;

namespace KickLib.Api.Unofficial.Clients.Puppeteer
{
    /// <summary>
    ///     Optimized authentication service that supports multiple users with shared browser instance
    /// </summary>
    public class OptimizedPuppeteerAuthenticationService : IAuthenticationService
    {
        private readonly BrowserSettings _browserSettings;
        private readonly ILogger _logger;
        private readonly string _sessionId;
        private readonly SessionManager _sessionManager;
        private readonly BrowserManager _browserManager;

        public string BearerToken => _sessionManager.GetSession(_sessionId)?.BearerToken;
        public string XsrfToken => _sessionManager.GetSession(_sessionId)?.XsrfToken;
        public bool IsAuthenticated => _sessionManager.GetSession(_sessionId)?.IsAuthenticated ?? false;

        public OptimizedPuppeteerAuthenticationService(
            string sessionId, 
            BrowserSettings browserSettings, 
            BrowserManager browserManager, 
            SessionManager sessionManager, 
            ILogger logger = null)
        {
            _sessionId = sessionId ?? throw new ArgumentNullException(nameof(sessionId));
            _logger = logger;
            _browserSettings = browserSettings ?? BrowserSettings.Empty;
            _sessionManager = sessionManager ?? throw new ArgumentNullException(nameof(sessionManager));
            _browserManager = browserManager ?? throw new ArgumentNullException(nameof(browserManager));
        }

        public async Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            _logger?.LogInformation("Starting authentication process for session {SessionId}. This might take a while...", _sessionId);

            var page = await _browserManager.GetOrCreateSessionPageAsync(_sessionId);

            try
            {
                // Get XSRF token
                var xsrfToken = await GetXsrfTokenAsync(page);
                
                // Update session with XSRF token
                _sessionManager.UpdateSessionAuth(_sessionId, null, xsrfToken);
                
                if (authenticationSettings.HasTokenOverride)
                {
                    // Update session with authentication data
                    UpdateSessionAuthData(authenticationSettings.BearerTokenOverride, xsrfToken, authenticationSettings);
                    _logger?.LogInformation("Authenticated using bearer token override!");
                    return;
                }

                // Get token provider data
                var tokenProviderResponse = await _browserManager.ExecuteFetchRequestAsync(
                    _sessionId,
                    "https://kick.com/kick-token-provider",
                    "GET",
                    null,
                    new Dictionary<string, string>
                    {
                        ["Accept"] = "application/json",
                        ["Content-Type"] = "application/json"
                    }
                );

                var tokenProviderResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(tokenProviderResponse);
                var tokenProvider = JToken.Parse(tokenProviderResult.body.ToString());

                // Construct login payload
                var payloadPrep = new ExpandoObject() as IDictionary<string, object>;
                payloadPrep.Add("isMobileRequest", true);
                payloadPrep.Add("email", authenticationSettings.Username);
                payloadPrep.Add("password", authenticationSettings.Password);
                payloadPrep.Add(tokenProvider["nameFieldName"]!.ToString(), "");
                payloadPrep.Add(tokenProvider["validFromFieldName"]!.ToString(), tokenProvider["encryptedValidFrom"]);
                
                if (authenticationSettings.UseOtp)
                {
                    var otp = GenerateTotp(authenticationSettings.TwoFactorAuthCode);
                    payloadPrep.Add("one_time_password", otp);
                }

                var loginPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payloadPrep);

                // Perform login using mobile endpoint
                var loginResponse = await _browserManager.ExecuteFetchRequestAsync(
                    _sessionId,
                    "https://kick.com/mobile/login",
                    "POST",
                    loginPayload,
                    new Dictionary<string, string>
                    {
                        ["Accept"] = "application/json",
                        ["Content-Type"] = "application/json",
                        ["X-Xsrf-Token"] = xsrfToken
                    }
                );

                var loginResult = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(loginResponse);
                var loginResponseBody = loginResult.body.ToString();

                if (loginResponseBody.Contains("CSRF token mismatch"))
                {
                    throw new ArgumentException("Something went wrong: CSRF token mismatch");
                }

                var loginStatus = (int?)loginResult.status;
                if (loginStatus is not null && loginStatus != 200)
                {
                    throw new ArgumentException($"Login failed with status code {loginStatus}. Response: {loginResponseBody}");
                }

                var parsedLoginResponse = JToken.Parse(loginResponseBody);
                var token = parsedLoginResponse["token"]?.ToString();
                var faRequiredValue = parsedLoginResponse["2fa_required"]?.ToString() ?? string.Empty;
                bool faRequired = bool.TryParse(faRequiredValue, out bool tempFaRequired) && tempFaRequired;

                if (faRequired)
                {
                    throw new ArgumentException("2FA is required! Cannot log-in. Provide a valid OTP code setup.");
                }

                if (loginResponseBody.Contains("Invalid OTP"))
                {
                    throw new ArgumentException("Generated OTP was not valid!");
                }

                if (!loginResponseBody.Contains("token"))
                {
                    throw new ArgumentException("Something went wrong: No token found in payload!");
                }

                // Update session with authentication data
                UpdateSessionAuthData(token, xsrfToken, authenticationSettings);

                _logger?.LogInformation("Successfully authenticated session {SessionId}!", _sessionId);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Authentication failed for session {SessionId}", _sessionId);
                throw;
            }
        }

        public async Task RefreshXsrfTokenAsync<TPage>(TPage targetPage)
        {
            if (targetPage is null || targetPage is not IPage page)
            {
                throw new ArgumentNullException(nameof(targetPage));
            }

            var xsrfToken = await GetXsrfTokenAsync(page);
            _sessionManager.UpdateSessionAuth(_sessionId, BearerToken, xsrfToken);
        }

        private async Task<string> GetXsrfTokenAsync(IPage page)
        {
            try
            {
                return await page.GetXsrfTokenAsync(_logger);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Failed to get XSRF token for session {SessionId}", _sessionId);
                throw;
            }
        }

        private static string GenerateTotp(string twoFaAuthCode)
        {
            if (string.IsNullOrWhiteSpace(twoFaAuthCode))
            {
                throw new ArgumentException($"Missing 2FA authentication code! You need to provide it using {nameof(AuthenticationSettings)}");
            }

            var secretKey = Base32Encoding.ToBytes(twoFaAuthCode);
            var totp = new Totp(secretKey);

            return totp.ComputeTotp();
        }
        
        
        private void UpdateSessionAuthData(string token, string xsrfToken, AuthenticationSettings authenticationSettings)
        {
            _sessionManager.UpdateSessionAuth(_sessionId, token, xsrfToken);

            var session = _sessionManager.GetSession(_sessionId);
            if (session != null)
            {
                session.AuthenticationSettings = authenticationSettings;
            }
        }
    }
}
