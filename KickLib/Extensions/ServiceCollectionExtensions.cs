using KickLib;

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
        ///     If you want to use the Unofficial/Private Kick API, use <see cref="AddUnofficialKickLib"/>.
        /// </summary>
        public static IServiceCollection AddKickLib(this IServiceCollection services)
        {
            return services
                .AddScoped<IKickApi, KickApi>();
        }
    }
}