using KickLib;
using KickLib.Api.Interfaces;
using KickLib.Auth;
using Polly;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    ///     Dependency injection extensions for KickLib.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        ///     Adds KickLib services for Official Kick API to the DI.
        ///
        ///     If you want to use the Unofficial/Private Kick API, use <see cref="M:AddUnofficialKickLib(IServiceCollection)" />.
        /// </summary>
        public static IServiceCollection AddKickLib(this IServiceCollection services)
        {
            return services
                .AddHttpClient(HttpConstants.HttpClientName)
#if NET8_0_OR_GREATER
                .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler
                {
                    PooledConnectionLifetime = TimeSpan.FromMinutes(5)
                })
#endif
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(3, retry => TimeSpan.FromMilliseconds(200 * retry)))
                .Services
                .AddScoped<IKickApi, KickApi>()
                .AddScoped<ApiSettings>(_ => ApiSettings.Default)
                .AddScoped<IAuthorization, KickLib.Api.Authorization>()
                .AddScoped<ICategories, KickLib.Api.Categories>()
                .AddScoped<IChannels, KickLib.Api.Channels>()
                .AddScoped<IChannelRewards, KickLib.Api.ChannelRewards>()
                .AddScoped<IChat, KickLib.Api.Chat>()
                .AddScoped<IEventSubscriptions, KickLib.Api.EventSubscriptions>()
                .AddScoped<IKicks, KickLib.Api.Kicks>()
                .AddScoped<ILivestreams, KickLib.Api.Livestreams>()
                .AddScoped<IModeration, KickLib.Api.Moderation>()
                .AddScoped<IUsers, KickLib.Api.Users>()
                .AddSingleton<IKickOAuthGenerator, KickOAuthGenerator>();
        }
    }
}