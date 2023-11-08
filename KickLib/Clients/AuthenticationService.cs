using System.Dynamic;
using KickLib.Extensions;
using KickLib.Interfaces;
using KickLib.Models;
using Newtonsoft.Json.Linq;
using OtpNet;
using PuppeteerSharp;

namespace KickLib.Clients;

/// <summary>
///     Custom authentication flow for Kick API.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly BrowserSettings _browserSettings;
    
    public string BearerToken { get; private set; }
    public string XsrfToken { get; private set; }
    public bool IsAuthenticated => BearerToken is not null;

    public AuthenticationService(BrowserSettings browserSettings)
    {
        _browserSettings = browserSettings ?? BrowserSettings.Empty;
    }
    
    public async Task AuthenticateAsync(AuthenticationSettings authenticationSettings)
    {
        await using var browser = await BrowserInitializer.LaunchBrowserAsync(_browserSettings);
        
        await using var page = await browser.NewPageAsync();
        var xsrfToken = await page.GetXsrfTokenAsync();
        XsrfToken = xsrfToken;

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
        ");
        
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
        var loginResponse = await page.EvaluateFunctionAsync<string>($@"
            async () => {{
                const response = await fetch('https://kick.com/mobile/login', {{
                    method: 'POST',
                    headers: {{
                        'Accept': 'application/json',
                        'Content-Type': 'application/json',
                        'X-Xsrf-Token': '{xsrfToken}'
                    }},
                    body: JSON.stringify({loginPayload})
                }});
                return response.text();
            }}
        ");

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
        Console.WriteLine("Successfully authenticated!");
    }

    public async Task RefreshXsrfTokenAsync(IPage targetPage)
    {
        if (targetPage is null)
        {
            throw new ArgumentNullException(nameof(targetPage));
        }
        
        XsrfToken = await targetPage.GetXsrfTokenAsync();
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