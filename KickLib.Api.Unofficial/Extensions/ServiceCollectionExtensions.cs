using KickLib.Api.Unofficial;
using KickLib.Api.Unofficial.Clients;
using KickLib.Api.Unofficial.Clients.CycleTls;
using KickLib.Api.Unofficial.Clients.Puppeteer;
using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Extensions;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models;
using KickLib.Client;
using KickLib.Client.Interfaces;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Dependency injection extensions for KickLib.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds KickLib services for Unofficial/Private Kick API to the DI.
        ///     You must also add a client implementation (e.g. <see cref="IKickLibBuilder.WithPuppeteerClient"/> or custom).
        /// </summary>
        public static IKickLibBuilder AddUnofficialKickLib(this IServiceCollection services)
        {
            services
                .AddScoped<IUnofficialKickApi, KickUnofficialApi>()
                .AddScoped<IKickClient, KickClient>();
        
            return new KickLibBuilder(services);
        }
    
        /// <summary>
        ///     Adds KickLib services for Unofficial/Private Kick API to the DI with optimized multi-user support.
        ///     Uses shared browser instance and session management for better performance.
        /// </summary>
        public static IKickLibBuilder AddOptimizedUnofficialKickLib(this IServiceCollection services)
        {
            services
                .AddSingleton<Core.BrowserManager>()
                .AddSingleton<Core.SessionManager>()
                .AddSingleton<IOptimizedKickUnofficialApiFactory, OptimizedKickUnofficialApiFactory>()
                .AddScoped<IKickClient, KickClient>();

            return new KickLibBuilder(services);
        }

        internal class KickLibBuilder : IKickLibBuilder
        {
            private readonly IServiceCollection _services;
            private bool _isClientAdded;

            public KickLibBuilder(IServiceCollection services)
            {
                _services = services;
            }
        
            public IServiceCollection WithTlsClient()
            {
                if (_isClientAdded)
                {
                    throw new InvalidOperationException("You can only add one KickLib client implementation.");    
                }
            
                // services.Configure<BrowserSettings>(configuration.GetSection(nameof(SpoofSettings)));
                _isClientAdded = true;
            
                return _services
                    .AddSingleton<IApiCaller, TlsSpoofClient>()
                    .AddScoped<IAuthenticationService, TlsSpoofAuthenticationService>()
                    .AddScoped<SpoofSettings>();
            }
    
            public IServiceCollection WithPuppeteerClient()
            {
                if (_isClientAdded)
                {
                    throw new InvalidOperationException("You can only add one KickLib client implementation.");    
                }
            
                // services.Configure<BrowserSettings>(configuration.GetSection(nameof(BrowserSettings)));
                _isClientAdded = true;
            
                return _services
                    .AddScoped<IApiCaller, BrowserClient>()
                    .AddScoped<IAuthenticationService, PuppeteerAuthenticationService>()
                    .AddScoped<BrowserSettings>();
            }

            public IServiceCollection WithOptimizedPuppeteerClient(BrowserSettings browserSettings = null)
            {
                if (_isClientAdded)
                {
                    throw new InvalidOperationException("You can only add one KickLib client implementation.");    
                }
            
                _isClientAdded = true;
                
                if (browserSettings != null)
                {
                    _services.AddSingleton(browserSettings);
                }
                else
                {
                    _services.AddScoped<BrowserSettings>();
                }
            
                return _services
                    .AddScoped<IApiCaller, OptimizedBrowserClient>()
                    .AddScoped<IAuthenticationService, OptimizedPuppeteerAuthenticationService>();
            }
        }
    }
}