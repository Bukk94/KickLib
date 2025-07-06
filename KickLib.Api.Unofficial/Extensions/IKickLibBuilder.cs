using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.DependencyInjection;

namespace KickLib.Api.Unofficial.Extensions
{
    /// <summary>
    ///     Dependency injection builder for KickLib.
    /// </summary>
    public interface IKickLibBuilder
    {
        /// <summary>
        ///     Adds TLS Spoofing client, that uses CycleTLS to impersonate JA3 Fingerprint.
        /// </summary>
        IServiceCollection WithTlsClient();
        
        /// <summary>
        ///     Adds client that uses Puppeteer to spin headless browser.
        /// </summary>
        IServiceCollection WithPuppeteerClient(BrowserSettings browserSettings = null);

        /// <summary>
        ///     Adds optimized client that uses shared Puppeteer browser instance with multi-user support.
        /// </summary>
        IServiceCollection WithSessionPuppeteerClient(BrowserSettings browserSettings = null);
    }
}