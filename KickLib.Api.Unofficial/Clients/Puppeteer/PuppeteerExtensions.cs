using System.Text.RegularExpressions;
using System.Web;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace KickLib.Api.Unofficial.Clients.Puppeteer
{
    public static class PuppeteerExtensions
    {
        public static async Task<string> GetXsrfTokenAsync(this IPage page, ILogger logger)
        {
            string xsrfToken = null;
            var attempts = 10;
            while (string.IsNullOrEmpty(xsrfToken) && attempts > 0)
            {
                // Go to Kick's main page
                var pageResponse = await page.GoToAsync(Constants.CsrfUrl).ConfigureAwait(false);
                await Task.Delay(500);
                var responseHeaders = pageResponse.Headers;

                if (!responseHeaders.ContainsKey("set-cookie"))
                {
                    logger?.LogError("Call to Kick.com did not return any set-cookie header! Cannot retrieve XSRF-Token. Retrying");
                    attempts--;
                    continue;
                }
            
                // Parse XSRF token
                var match = Regex.Match(responseHeaders["set-cookie"], "XSRF-TOKEN=(?<token>[^;]*)");
                if (!match.Success)
                {
                    logger?.LogInformation($"Failed attempt to get XSRF Token. Remaining attemts: {attempts}.");
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
}