using KickLib.Api;
using Microsoft.Extensions.Logging;

namespace KickLib;

public class KickApi : IKickApi
{
    /// <inheritdoc />
    public Authorization Authorization { get; }
    
    /// <inheritdoc />
    public Categories Categories { get; }
    
    /// <inheritdoc />
    public Chat Chat { get; }
    
    /// <inheritdoc />
    public Channels Channels { get; }
    
    /// <inheritdoc />
    public Users Users { get; }

    public KickApi(
        ApiSettings settings,
        ILogger<KickApi>? logger = null)
    {
        logger ??= LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<KickApi>();
        Authorization = new Authorization(settings, logger);
        Categories = new Categories(settings, logger);
        Chat = new Chat(settings, logger);
        Channels = new Channels(settings, logger);
        Users = new Users(settings, logger);
    }
}