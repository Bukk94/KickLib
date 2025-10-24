using System.Dynamic;
using System.Text.RegularExpressions;
using System.Web;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OtpNet;

namespace KickLib.Api.Unofficial.Clients.CycleTls
{
    public class TlsSpoofAuthenticationService : IAuthenticationService
    {
        private readonly ILogger _logger;

        /// <inheritdoc />
        public string BearerToken { get; private set; }

        /// <inheritdoc />
        public string XsrfToken { get; private set; }

        /// <inheritdoc />
        public bool IsAuthenticated => BearerToken is not null;

        public TlsSpoofAuthenticationService(ILogger logger = null)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
        {
            _logger?.LogInformation("Starting authentication process. This might take a while...");

            var xsrfToken = await GetXsrfTokenAsync().ConfigureAwait(false);
            XsrfToken = xsrfToken;

            if (authenticationSettings.HasTokenOverride)
            {
                BearerToken = authenticationSettings.BearerTokenOverride;
                _logger?.LogInformation("Authenticated using bearer token override!");
                return;
            }
            
            // Call kick-token-provider to get data required for login process
            var tokenProviderOptions = CycleTlsInitializer.GetOptions("https://kick.com/kick-token-provider");
            tokenProviderOptions.Method = "GET";
            var response = await CycleTlsInitializer.Client.SendAsync(tokenProviderOptions).ConfigureAwait(false);

            var tokenProviderResponse = response.Body;
            var tokenProvider = JToken.Parse(tokenProviderResponse);

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

            // Using mobile login is easier
            var loginOptions = CycleTlsInitializer.GetOptions("https://kick.com/mobile/login");
            loginOptions.Method = "POST";
            loginOptions.Body = loginPayload;
            loginOptions.Headers.Add("X-Xsrf-Token", xsrfToken);
            var loginResponseData = await CycleTlsInitializer.Client.SendAsync(loginOptions).ConfigureAwait(false);

            var loginResponse = loginResponseData.Body;

            if (loginResponse.Contains("CSRF token mismatch"))
            {
                throw new ArgumentException("Something went wrong: CSRF token mismatch");
            }

            var parsedLoginResponse = JToken.Parse(loginResponse);

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
            if (targetPage is null)
            {
                throw new ArgumentNullException(nameof(targetPage));
            }

            XsrfToken = await GetXsrfTokenAsync().ConfigureAwait(false);
        }

        private async Task<string> GetXsrfTokenAsync()
        {
            var options = CycleTlsInitializer.GetOptions(Constants.CsrfUrl);
            var xsrfTokenResponse = await CycleTlsInitializer.Client.SendAsync(options).ConfigureAwait(false);

            if (xsrfTokenResponse.Status != 200)
            {
                throw new ArgumentException("Failed to retrieve XSRF Token");
            }

            // Parse XSRF token
            var match = Regex.Match(xsrfTokenResponse.Headers["Set-Cookie"], "XSRF-TOKEN=(?<token>[^;]*)");
            if (!match.Success)
            {
                throw new ArgumentException("Failed to retrieve XSRF Token");
            }

            return HttpUtility.UrlDecode(match.Groups["token"].Value);
        }

        private static string GenerateTotp(string twoFaAuthCode)
        {
            if (string.IsNullOrWhiteSpace(twoFaAuthCode))
            {
                throw new ArgumentException(
                    $"Missing 2FA authentication code! You need to provide it using {nameof(AuthenticationSettings)}");
            }

            var secretKey = Base32Encoding.ToBytes(twoFaAuthCode);
            var totp = new Totp(secretKey);

            return totp.ComputeTotp();
        }
    }
}