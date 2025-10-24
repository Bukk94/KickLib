using KickLib.Api.Unofficial.Models;
using PuppeteerExtraSharp;
using PuppeteerSharp;

namespace KickLib.Api.Unofficial.Clients.Puppeteer
{
    public static class BrowserInitializer
    {
        private static async Task EnsureBrowserAsync(BrowserSettings settings)
        {
#if NET8_0_OR_GREATER
            var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Platform = settings.BrowserPlatform,
                Path = settings.BrowserDownloadPath
            });
#else
            using var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Platform = settings.BrowserPlatform,
                Path = settings.BrowserDownloadPath
            });
#endif

            await browserFetcher.DownloadAsync();
        }
    
        public static async Task<IBrowser> LaunchBrowserAsync(BrowserSettings settings)
        {
            if (settings.EnableBrowserFetching)
            {
                await EnsureBrowserAsync(settings);
            }

            var extra = new PuppeteerExtra();
            foreach (var plugin in settings.PuppeteerPlugins)
            {
                extra.Use(plugin);
            }
            
            return await extra.LaunchAsync(settings.LaunchOptions);
        }
    }
}