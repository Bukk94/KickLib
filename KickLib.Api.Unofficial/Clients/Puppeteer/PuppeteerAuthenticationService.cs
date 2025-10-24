using System.Dynamic;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OtpNet;
using PuppeteerSharp;

namespace KickLib.Api.Unofficial.Clients.Puppeteer
{
    /// <summary>
    ///     Custom authentication flow for Kick API.
    /// </summary>
    public class PuppeteerAuthenticationService : IAuthenticationService
    {
        private readonly BrowserSettings _browserSettings;
        private readonly ILogger _logger;

        /// <inheritdoc />
        public string BearerToken { get; private set; }

        /// <inheritdoc />
        public string XsrfToken { get; private set; }

        /// <inheritdoc />
        public bool IsAuthenticated => BearerToken is not null;

        public PuppeteerAuthenticationService(BrowserSettings browserSettings, ILogger logger = null)
        {
            _logger = logger;
            _browserSettings = browserSettings ?? BrowserSettings.Empty;
        }
    
        public async Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            _logger?.LogInformation("Starting authentication process. This might take a while...");

            await using var browser = await BrowserInitializer.LaunchBrowserAsync(_browserSettings).ConfigureAwait(false);
        
            await using var page = await browser.NewPageAsync().ConfigureAwait(false);
            var xsrfToken = await page.GetXsrfTokenAsync(_logger).ConfigureAwait(false);
            XsrfToken = xsrfToken;

            if (authenticationSettings.HasTokenOverride)
            {
                BearerToken = authenticationSettings.BearerTokenOverride;
                _logger?.LogInformation("Authenticated using bearer token override!");
                return;
            }
            
            // Call kick-token-provider to get data required for login process
            var tokenProviderResponse = await page.EvaluateFunctionAsync<string>(@"
            async () => {
                const response = await fetch('https://kick.com/kick-token-provider', {
                    method: 'GET',
                    headers: {
                        'Accept': 'application/json',
                        'Content-Type': 'application/json'
                    }
                });

                return response.text();
            }
            ").ConfigureAwait(false);
        
            JToken tokenProvider;
            try
            {
                tokenProvider = JToken.Parse(tokenProviderResponse);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to request token provider. Actual response: {tokenProviderResponse}", ex);
            }

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

            var randomUa = UserAgentRandomizer.GetRandomUserAgent();
            
            // Using mobile login is easier  
            var loginResponseRaw = await page.EvaluateFunctionAsync<string>($@"
            async () => {{
                const response = await fetch('https://kick.com/mobile/login', {{
                    method: 'POST',
                    headers: {{
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'Referer': 'https://kick.com/',
                        'Origin': 'https://kick.com',
                        'User-Agent': '{randomUa}',
                        'X-Xsrf-Token': '{xsrfToken}'
                    }},
                    body: JSON.stringify({loginPayload})
                }});
                const body = await response.text();
                return JSON.stringify({{ status: response.status, body }});
            }}
            ").ConfigureAwait(false);

            var loginResponseObj = JObject.Parse(loginResponseRaw);
            var loginResponse = loginResponseObj["body"]?.ToString();
            var loginStatus = loginResponseObj["status"]?.ToObject<int>() ?? 0;

            if (loginStatus != 200)
            {
                throw new ArgumentException($"Login request failed with status code {loginStatus}. Response: {loginResponse}");
            }

            JToken parsedLoginResponse;
            try
            {
                parsedLoginResponse = JToken.Parse(loginResponse);
            } 
            catch (Exception ex)
            {
                throw new ArgumentException($"Failed to perform login request. Actual response: {loginResponse}", ex);
            }
        
            var token = parsedLoginResponse["token"]?.ToString();
            bool.TryParse(parsedLoginResponse["2fa_required"]?.ToString() ?? string.Empty, out var faRequired);

            if (faRequired)
            {
                throw new ArgumentException("2FA is required! Cannot log-in. Provide a valid OTP code setup.");
            }

            if (loginResponse.Contains("Invalid OTP"))
            {
                throw new ArgumentException("Generated OTP was not valid!");
            }

            if (!loginResponse.Contains("token"))
            {
                throw new ArgumentException("Something went wrong: No token found in payload!");
            }

            BearerToken = token;
            _logger?.LogInformation("Successfully authenticated!");
        }

        public async Task RefreshXsrfTokenAsync<TPage>(TPage targetPage)
        {
            if (targetPage is null ||
                targetPage is not IPage page)
            {
                throw new ArgumentNullException(nameof(targetPage));
            }

            XsrfToken = await page.GetXsrfTokenAsync(_logger).ConfigureAwait(false);
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
    }
}