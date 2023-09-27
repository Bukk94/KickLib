using KickLib;
using KickLib.Client;
using KickLib.Client.Interfaces;
using KickLib.Clients;
using KickLib.Interfaces;

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
}