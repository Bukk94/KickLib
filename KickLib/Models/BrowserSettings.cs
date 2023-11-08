namespace KickLib.Models;

public class BrowserSettings
{
    /// <summary>
    ///     Automatically downloads browser.
    /// </summary>
    public bool EnableBrowserFetching { get; set; } = true;

    /// <summary>
    ///     Executable path to Puppeteer headless browser.
    ///     If empty, default will be taken. 
    /// </summary>
    public string BrowserExecutablePath { get; set; }

    public PuppeteerSharp.Platform? BrowserPlatform { get; set; }
    
    /// <summary>
    ///     Puppeteer browser download path. Defaults to [root]/.local-chromium
    /// </summary>
    public string BrowserDownloadPath { get; set; }
    
    public static BrowserSettings Empty => new();
}