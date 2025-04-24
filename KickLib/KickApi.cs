using KickLib.Api;
using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Extensions;
using Microsoft.Extensions.Logging;

namespace KickLib;

/// <summary>
///     Concrete implementation of the Kick API.
/// </summary>
public class KickApi : IKickApi
{
    /// <inheritdoc />
    public IAuthorization Authorization { get; }
    
    /// <inheritdoc />
    public ICategories Categories { get; }
    
    /// <inheritdoc />
    public IChat Chat { get; }
    
    /// <inheritdoc />
    public IChannels Channels { get; }
    
    /// <inheritdoc />
    public IEventSubscriptions EventSubscriptions { get; }
    
    /// <inheritdoc />
    public ILivestreams Livestreams { get; }
    
    /// <inheritdoc />
    public IUsers Users { get; }
    
    /// <inheritdoc />
    public ApiSettings ApiSettings { get; }

    /// <summary>
    ///     Create a new instance of the KickLib.
    /// </summary>
    /// <param name="authorization">Authorization api implementation.</param>
    /// <param name="categories">Category api implementation.</param>
    /// <param name="channels">Channel api implementation.</param>
    /// <param name="chat">Chat api implementation.</param>
    /// <param name="eventSubscriptions">Event subscription api implementation.</param>
    /// <param name="livestreams">Live stream api implementation.</param>
    /// <param name="users">User api implementation.</param>
    /// <param name="settings">API Settings.  {(If null, default settings will be used}.</param>
    public KickApi(
        IAuthorization authorization,
        ICategories categories,
        IChat chat,
        IChannels channels,
        IEventSubscriptions eventSubscriptions,
        ILivestreams livestreams,
        IUsers users,
        ApiSettings? settings = null)
    {
        ApiSettings = settings ?? ApiSettings.Default;

        // APIs
        Authorization = authorization;
        Categories = categories;
        Chat = chat;
        Channels = channels;
        EventSubscriptions = eventSubscriptions;
        Livestreams = livestreams;
        Users = users;
    }

    /// <summary>
    ///     Creates an instance of the Kick API.
    /// </summary>
    /// <param name="settings">Optional API settings (if not provided, defaults will be used).</param>
    /// <param name="loggerFactory">Logger factory for logging (if not provided, NullLogger will be used).</param>
    public static IKickApi Create(
        ApiSettings? settings = null,
        ILoggerFactory? loggerFactory = null)
    {
        var apiSettings = settings ?? ApiSettings.Default;
        var oauthGenerator = new KickOAuthGenerator();
        var clientFactory = new KickLibHttpClientFactory();
        
        return new KickApi(
            new Authorization(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<Authorization>()),
            new Categories(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<Categories>()),
            new Chat(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<Chat>()),
            new Channels(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<Channels>()),
            new EventSubscriptions(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<EventSubscriptions>()),
            new Livestreams(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<Livestreams>()),
            new Users(apiSettings, oauthGenerator, clientFactory, loggerFactory.GetLogger<Users>()),
            apiSettings);
    }
}