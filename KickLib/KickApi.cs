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
    public IChannelRewards ChannelRewards { get; }
    
    /// <inheritdoc />
    public IEventSubscriptions EventSubscriptions { get; }
    
    /// <inheritdoc />
    public ILivestreams Livestreams { get; }
    
    /// <inheritdoc />
    public IKicks Kicks { get; }
    
    /// <inheritdoc />
    public IModeration Moderation { get; }
    
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
    /// <param name="channelRewards">Channel Rewards api implementation.</param>
    /// <param name="chat">Chat api implementation.</param>
    /// <param name="eventSubscriptions">Event subscription api implementation.</param>
    /// <param name="livestreams">Live stream api implementation.</param>
    /// <param name="kicks">Kicks api implementation.</param>
    /// <param name="moderation">Moderation api implementation.</param>
    /// <param name="users">User api implementation.</param>
    /// <param name="settings">API Settings. {(If null, default settings will be used}.</param>
    public KickApi(
        IAuthorization authorization,
        ICategories categories,
        IChat chat,
        IChannels channels,
        IChannelRewards channelRewards,
        IEventSubscriptions eventSubscriptions,
        ILivestreams livestreams,
        IKicks kicks,
        IModeration moderation,
        IUsers users,
        ApiSettings? settings = null)
    {
        ApiSettings = settings ?? ApiSettings.Default;

        // APIs
        Authorization = authorization;
        Categories = categories;
        Chat = chat;
        Channels = channels;
        ChannelRewards = channelRewards;
        EventSubscriptions = eventSubscriptions;
        Livestreams = livestreams;
        Kicks = kicks;
        Moderation = moderation;
        Users = users;
    }

    /// <summary>
    ///     Creates an instance of the Kick API.
    /// </summary>
    /// <param name="settings">Optional API settings (if not provided, defaults will be used).</param>
    /// <param name="loggerFactory">Logger factory for logging (if not provided, NullLogger will be used).</param>
    /// <param name="httpClientFactory">Optional IHttpClientFactory to be used for HTTP calls (if not provided, defaults will be used).</param>
    public static IKickApi Create(
        ApiSettings? settings = null,
        ILoggerFactory? loggerFactory = null,
        IHttpClientFactory? httpClientFactory = null)
    {
        var apiSettings = settings ?? ApiSettings.Default;
        httpClientFactory ??= new KickLibHttpClientFactory();
        var oauthGenerator = new KickOAuthGenerator(httpClientFactory);
        
        return new KickApi(
            new Authorization(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Authorization>()),
            new Categories(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Categories>()),
            new Chat(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Chat>()),
            new Channels(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Channels>()),
            new ChannelRewards(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<ChannelRewards>()),
            new EventSubscriptions(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<EventSubscriptions>()),
            new Livestreams(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Livestreams>()),
            new Kicks(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Kicks>()),
            new Moderation(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Moderation>()),
            new Users(apiSettings, oauthGenerator, httpClientFactory, loggerFactory.GetLogger<Users>()),
            apiSettings);
    }
}