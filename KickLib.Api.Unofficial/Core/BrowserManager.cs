using KickLib.Api.Unofficial.Models;
using Microsoft.Extensions.Logging;
using PuppeteerExtraSharp;
using PuppeteerSharp;
using System.Collections.Concurrent;

namespace KickLib.Api.Unofficial.Core
{
    /// <summary>
    ///     Browser manager to optimize Puppeteer usage.
    /// </summary>
    public class BrowserManager : IDisposable
    {
        private static readonly SemaphoreSlim InitializationSemaphore = new(1, 1);

        private IBrowser _browser;
        private readonly BrowserSettings _settings;
        private readonly ILogger _logger;
        private readonly ConcurrentDictionary<string, IPage> _sessionPages = new();
        private bool _disposed;

        public BrowserManager(
            BrowserSettings settings = null, 
            ILogger<BrowserManager> logger = null)
        {
            _settings = settings ?? BrowserSettings.Empty;
            _logger = logger;
        }

        public async Task<IBrowser> GetBrowserAsync()
        {
            if (_browser == null || _browser.IsClosed)
            {
                await InitializationSemaphore.WaitAsync();
                try
                {
                    if (_browser == null || _browser.IsClosed)
                    {
                        _logger?.LogInformation("Initializing shared browser instance...");
                        
                        if (_settings.EnableBrowserFetching)
                        {
                            await EnsureBrowserAsync();
                        }

                        var extra = new PuppeteerExtra();
                        foreach (var plugin in _settings.PuppeteerPlugins)
                        {
                            extra.Use(plugin);
                        }

                        _browser = await extra.LaunchAsync(_settings.LaunchOptions);
                        _logger?.LogInformation("Shared browser instance initialized successfully");
                    }
                }
                finally
                {
                    InitializationSemaphore.Release();
                }
            }

            return _browser;
        }

        public async Task<IPage> GetOrCreateSessionPageAsync(string sessionId)
        {
            if (_sessionPages.TryGetValue(sessionId, out var existingPage) && !existingPage.IsClosed)
            {
                return existingPage;
            }

            var browser = await GetBrowserAsync();
            var page = await browser.NewPageAsync();
            
            // Set up page optimization
            await page.SetCacheEnabledAsync(true);
            await page.SetRequestInterceptionAsync(true);
            
            // Block unnecessary resources to improve performance
            page.Request += async (sender, e) =>
            {
                var request = e.Request;
                var resourceType = request.ResourceType;
                
                // Block images, stylesheets, fonts to improve performance
                if (resourceType == ResourceType.Image || 
                    resourceType == ResourceType.StyleSheet || 
                    resourceType == ResourceType.Font ||
                    resourceType == ResourceType.Media)
                {
                    await request.AbortAsync();
                }
                else
                {
                    await request.ContinueAsync();
                }
            };

            _sessionPages.TryAdd(sessionId, page);
            return page;
        }

        public async Task<string> ExecuteFetchRequestAsync(string sessionId, string url, string method = "GET", string body = null, Dictionary<string, string> headers = null)
        {
            var page = await GetOrCreateSessionPageAsync(sessionId);
            
            var headersJson = headers != null 
                ? Newtonsoft.Json.JsonConvert.SerializeObject(headers) 
                : "{}";

            var script = $@"
                async () => {{
                    try {{
                        const response = await fetch('{url}', {{
                            method: '{method}',
                            headers: {headersJson},
                            {(body != null ? $"body: {Newtonsoft.Json.JsonConvert.SerializeObject(body)}," : "")}
                        }});
                        
                        const text = await response.text();
                        return {{
                            status: response.status,
                            statusText: response.statusText,
                            body: text,
                            headers: Object.fromEntries(response.headers.entries())
                        }};
                    }} catch (error) {{
                        return {{
                            status: 0,
                            statusText: error.message,
                            body: '',
                            headers: {{}}
                        }};
                    }}
                }}
            ";

            var result = await page.EvaluateFunctionAsync<dynamic>(script);
            return Newtonsoft.Json.JsonConvert.SerializeObject(result);
        }

        public void RemoveSession(string sessionId)
        {
            if (_sessionPages.TryRemove(sessionId, out var page))
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        if (!page.IsClosed)
                        {
                            await page.CloseAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Error closing session page for {SessionId}", sessionId);
                    }
                });
            }
        }

        private async Task EnsureBrowserAsync()
        {
#if NET8_0_OR_GREATER
            var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Platform = _settings.BrowserPlatform,
                Path = _settings.BrowserDownloadPath
            });
#else
            using var browserFetcher = new BrowserFetcher(new BrowserFetcherOptions
            {
                Platform = _settings.BrowserPlatform,
                Path = _settings.BrowserDownloadPath
            });
#endif
            await browserFetcher.DownloadAsync();
        }

        public void Dispose()
        {
            if (_disposed) return;

            foreach (var page in _sessionPages.Values)
            {
                try
                {
                    if (!page.IsClosed)
                    {
                        page.CloseAsync().Wait(TimeSpan.FromSeconds(5));
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Error closing page during disposal");
                }
            }

            _sessionPages.Clear();

            try
            {
                if (_browser != null && !_browser.IsClosed)
                {
                    _browser.CloseAsync().Wait(TimeSpan.FromSeconds(10));
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error closing browser during disposal");
            }

            _disposed = true;
        }
    }
}
