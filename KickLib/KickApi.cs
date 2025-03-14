using KickLib.Api;
using Microsoft.Extensions.Logging;

namespace KickLib;

/// <summary>
///     Concrete implementation of the Kick API.
/// </summary>
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
    public EventSubscriptions EventSubscriptions { get; }
    
    /// <inheritdoc />
    public Users Users { get; }
    
    /// <inheritdoc />
    public ApiSettings ApiSettings { get; }

    public KickApi(
        ApiSettings settings,
        ILogger<KickApi>? logger = null)
    {
        ArgumentNullException.ThrowIfNull(settings);
        ApiSettings = settings;
        logger ??= LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<KickApi>();
        
        // APIs
        Authorization = new Authorization(settings, logger);
        Categories = new Categories(settings, logger);
        Chat = new Chat(settings, logger);
        Channels = new Channels(settings, logger);
        EventSubscriptions = new EventSubscriptions(settings, logger);
        Users = new Users(settings, logger);
    }
}