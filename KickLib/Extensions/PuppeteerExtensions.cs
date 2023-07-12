using System.Text.RegularExpressions;
using System.Web;
using PuppeteerSharp;

namespace KickLib.Extensions;

public static class PuppeteerExtensions
{
    public static async Task<string> GetXsrfTokenAsync(this IPage page)
    {
        string xsrfToken = null;
        var attempts = 10;
        while (string.IsNullOrEmpty(xsrfToken) && attempts > 0)
        {
            // Go to Kick's main page
            var pageResponse = await page.GoToAsync(Constants.KickUrl);
            await Task.Delay(500);
            var responseHeaders = pageResponse.Headers;
            
            // Parse XSRF token
            var match = Regex.Match(responseHeaders["set-cookie"], "XSRF-TOKEN=(?<token>[^;]*)");
            if (!match.Success)
            {
                Console.WriteLine("Failed attempt to get XSRF Token");
                attempts--;
                continue;
            }
            
            xsrfToken = HttpUtility.UrlDecode(match.Groups["token"].Value);
        }

        if (string.IsNullOrWhiteSpace(xsrfToken))
        {
            throw new ArgumentException("Failed to retrieve XSRF Token");
        }
        
        return xsrfToken;
    }
}