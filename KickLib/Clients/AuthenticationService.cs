using System.Dynamic;
using KickLib.Extensions;
using KickLib.Interfaces;
using Newtonsoft.Json.Linq;
using PuppeteerSharp;

namespace KickLib.Clients;

/// <summary>
///     Custom authentication flow for Kick API.
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    public string BearerToken { get; private set; }
    public string XsrfToken { get; private set; }
    public bool IsAuthenticated => BearerToken is not null;
    
    public async Task AuthenticateAsync(string username, string password, string totp)
    {
        await using var browser = await BrowserInitializer.LaunchBrowserAsync();
        
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
        payloadPrep.Add("email", username);
        payloadPrep.Add("password", password);
        payloadPrep.Add("one_time_password", totp);
        payloadPrep.Add(tokenProvider["nameFieldName"]!.ToString(), "");
        payloadPrep.Add(tokenProvider["validFromFieldName"]!.ToString(), tokenProvider["encryptedValidFrom"]);
        
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

        if (!loginResponse.Contains("token"))
        {
            throw new ArgumentException("Something went wrong: No token found in payload!");
        }

        var parsedLoginResponse = JToken.Parse(loginResponse);
        
        var token = parsedLoginResponse["token"]?.ToString();
        bool.TryParse(parsedLoginResponse["2fa_required"]?.ToString() ?? string.Empty, out var faRequired);

        if (faRequired)
        {
            throw new ArgumentException("2FA is required! Cannot log-in.");
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
}