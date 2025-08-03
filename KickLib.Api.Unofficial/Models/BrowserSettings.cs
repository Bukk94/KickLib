using KickLib.Api.Unofficial.Clients.Puppeteer;
using PuppeteerExtraSharp.Plugins;
using PuppeteerExtraSharp.Plugins.ExtraStealth;
using PuppeteerSharp;

namespace KickLib.Api.Unofficial.Models
{
    public class BrowserSettings
    {
        /// <summary>
        ///     Automatically downloads browser.
        /// </summary>
        public bool EnableBrowserFetching { get; set; } = true;

        public Platform? BrowserPlatform { get; set; }
    
        /// <summary>
        ///     Puppeteer browser download path. Defaults to [root]/.local-chromium
        /// </summary>
        public string BrowserDownloadPath { get; set; }

        /// <summary>
        ///     Retry delay for failed requests.
        /// </summary>
        public TimeSpan RetryDelay { get; set; } = TimeSpan.FromSeconds(5);
        
        /// <summary>
        ///     Number of times to retry a request before failing.
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        ///     Sets the puppeteer plugins to be added when initializing the browser.
        /// </summary>
        public IEnumerable<PuppeteerExtraPlugin> PuppeteerPlugins { get; set; } =
            new List<PuppeteerExtraPlugin>
            {
                new StealthPlugin(),
                new RandomUAPlugin()
            };
    
        /// <summary>
        ///     Sets the Launch options to be added when initializing the browser.
        /// </summary>
        public LaunchOptions LaunchOptions { get; set; } = 
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
                },
            };

        public static BrowserSettings Empty => new();
    }
}