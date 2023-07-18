using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;

namespace KickLib.Clients
{
    public static class BrowserInitializer
    {
        private static async Task EnsureBrowserAsync()
        {
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
        }
    
        public static async Task<IBrowser> LaunchBrowserAsync()
        {
            await EnsureBrowserAsync();

            var extra = new PuppeteerExtra();
            extra.Use(new StealthPlugin());
            extra.Use(new AnonymizeUaPlugin());

            return await extra.LaunchAsync(
                new LaunchOptions
                {
                    Headless = true,
                    Args = new[]
                    {
                        "--disable-gpu",
                        "--disable-dev-shm-usage",
                        "--disable-setuid-sandbox",
                        "--no-sandbox"
                    },
                    IgnoredDefaultArgs = new[]
                    {
                        "--disable-extensions"
                    }
                });
        }
    }
}