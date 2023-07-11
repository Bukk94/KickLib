using System.Text.RegularExpressions;
using KickLib.Interfaces;
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

    private static async Task EnsureBrowserAsync()
    {
        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
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
}