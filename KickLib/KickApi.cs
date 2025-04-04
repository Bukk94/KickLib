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
    public Livestreams Livestreams { get; }
    
    /// <inheritdoc />
    public Users Users { get; }
    
    /// <inheritdoc />
    public ApiSettings ApiSettings { get; }

    /// <summary>
    ///     Create a new instance of the KickLib with default settings.
    /// </summary>
    public KickApi()
        : this(ApiSettings.Default)
    {
    }
    
    /// <summary>
    ///     Create a new instance of the KickLib with custom settings.
    /// </summary>
    /// <param name="settings">API Settings.</param>
    /// <param name="logger">Logger (if null, default Console logger will be created).</param>
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
        Livestreams = new Livestreams(settings, logger);
        Users = new Users(settings, logger);
    }
}