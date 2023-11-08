using KickLib.Models;
using PuppeteerExtraSharp;
using PuppeteerExtraSharp.Plugins.AnonymizeUa;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;

namespace KickLib.Clients
{
    public static class BrowserInitializer
    {
        private static async Task EnsureBrowserAsync(BrowserSettings settings)
        {
            using var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Platform = settings.BrowserPlatform,
                Path = settings.BrowserDownloadPath
            });
            
            await browserFetcher.DownloadAsync();
        }
    
        public static async Task<IBrowser> LaunchBrowserAsync(BrowserSettings settings)
        {
            if (settings.EnableBrowserFetching)
            {
                await EnsureBrowserAsync(settings);
            }

            var extra = new PuppeteerExtra();
            extra.Use(new StealthPlugin());
            extra.Use(new AnonymizeUaPlugin());

            var launchSettings = new LaunchOptions
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
            };

            if (settings.BrowserExecutablePath is not null)
            {
                launchSettings.ExecutablePath = settings.BrowserExecutablePath;
            } 
            
            return await extra.LaunchAsync(launchSettings);
        }
    }
}