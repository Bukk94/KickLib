using System.Text.RegularExpressions;
using PuppeteerExtraSharp.Plugins;
using PuppeteerSharp;

namespace KickLib.Api.Unofficial.Clients.Puppeteer
{
    public class RandomUAPlugin : PuppeteerExtraPlugin
    {
        private static readonly Regex Regex = new("/\\(([^)]+)\\)/");

        public RandomUAPlugin()
            : base("random-ua")
        {
        }

#if NET8_0_OR_GREATER
        protected override async Task OnPageCreatedAsync(IPage page)
        {
            var randomUa = UserAgentRandomizer.GetRandomUserAgent();
            var userAgent = Regex.Replace(
                (await page.Browser.GetUserAgentAsync()).Replace("HeadlessChrome", "Chrome"),
                $"({randomUa})");

            await page.SetUserAgentAsync(userAgent);
        }
#else
            public override async Task OnPageCreated(IPage page)
            {
                var randomUa = UserAgentRandomizer.GetRandomUserAgent();
                var userAgent = Regex.Replace(
                    (await page.Browser.GetUserAgentAsync()).Replace("HeadlessChrome", "Chrome"), 
                    $"({randomUa})");

                await page.SetUserAgentAsync(userAgent);
            }
#endif
    }
}