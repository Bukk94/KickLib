using System.Text.RegularExpressions;
using KickLib.Extensions;
using KickLib.Interfaces;
using Newtonsoft.Json.Linq;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
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

    public BrowserClient(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    public Task AuthenticateAsync(string username, string password)
    {
        return _authenticationService.AuthenticateAsync(username, password);
    }
    
    public async Task<KeyValuePair<int, string>> SendRequestAsync(string url)
    {
        await EnsureBrowserAsync();
        
        var extra = new PuppeteerExtra(); 
        extra.Use(new StealthPlugin());
        extra.Use(new AnonymizeUaPlugin());
        
        await using var browser = await extra.LaunchAsync(
            new LaunchOptions
            {
                Headless = true
            });
        
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
        
        await EnsureBrowserAsync();
        
        var extra = new PuppeteerExtra(); 
        extra.Use(new StealthPlugin());
        extra.Use(new AnonymizeUaPlugin());
        
        await using var browser = await extra.LaunchAsync(
            new LaunchOptions
            {
                Headless = true
            });
        
        try
        {
            await using var page = await browser.NewPageAsync();
            var xsrfToken = await page.GetXsrfTokenAsync();
            
            var method = payload is not null ? "POST" : "GET";
            var body = payload is not null
                ? $", body: JSON.stringify({payload})"
                : "";
            
            var response = await page.EvaluateFunctionAsync<string>($@"
                async () => {{
                    const response = await fetch('{url}', {{
                        method: '{method}',
                        headers: {{
                            'Accept': 'application/json',
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer {_authenticationService.BearerToken}',
                            'X-Xsrf-Token': '{xsrfToken}'
                        }}{body}
                    }});
                    return response.text();
                }}
            ");

            if (response.Contains("CSRF token mismatch"))
            {
                throw new ArgumentException("Something went wrong: CSRF token mismatch");
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
    
    private static async Task EnsureBrowserAsync()
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
    }
}