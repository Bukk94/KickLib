using KickLib;
using KickLib.Client;
using KickLib.Client.Interfaces;
using KickLib.Clients;
using KickLib.Clients.CycleTls;
using KickLib.Clients.Puppeteer;
using KickLib.Extensions;
using KickLib.Interfaces;
using KickLib.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
///     Dependency injection extensions for KickLib.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Adds basic KickLib services to the DI.
    ///     You must also add a client implementation (e.g. <see cref="IKickLibBuilder.WithPuppeteerClient"/> or custom).
    /// </summary>
    public static IKickLibBuilder AddKickLib(this IServiceCollection services)
    {
        services
            .AddScoped<IKickApi, KickApi>()
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
    }
}