using KickLib.Api;
using Microsoft.Extensions.Logging;

namespace KickLib;

public class KickApi : IKickApi
{
    /// <inheritdoc />
    public Categories Categories { get; }
    
    /// <inheritdoc />
    public Channels Channels { get; }
    
    /// <inheritdoc />
    public Users Users { get; }

    public KickApi(
        ApiSettings settings,
        ILogger<KickApi> logger)
    {
        Categories = new Categories(settings, logger);
        Channels = new Channels(settings, logger);
        Users = new Users(settings, logger);
    }
}