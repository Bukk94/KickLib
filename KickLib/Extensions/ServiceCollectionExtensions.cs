using KickLib;
using KickLib.Client;
using KickLib.Client.Interfaces;
using KickLib.Clients;
using KickLib.Interfaces;
using KickLib.Models;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddKickLib(this IServiceCollection services)
    {
        return services
            .AddScoped<IKickApi, KickApi>()
            .AddScoped<IApiCaller, BrowserClient>()
            .AddScoped<IAuthenticationService, AuthenticationService>()
            .AddScoped<IKickClient, KickClient>();
    }
    
    public static IServiceCollection AddKickLibSettings(this IServiceCollection services)
    {
        return services
            .AddScoped<BrowserSettings>();
    }
}