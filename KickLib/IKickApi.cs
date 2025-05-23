using KickLib.Api.Interfaces;

namespace KickLib;

/// <summary>
///     The Kick API interface. This interface provides access to all the APIs available in the KickLib.
/// </summary>
public interface IKickApi
{
    /// <summary>
    ///     Authorization-related APIs to allow you to interact with OAuth endpoints.
    /// </summary>
    public IAuthorization Authorization { get; }

    /// <summary>
    ///     Categories APIs allow you to use and interact with the categories that are available on the Kick website.
    /// </summary>
    public ICategories Categories { get; }
    
    /// <summary>
    ///     Chat APIs allow you to use and interact with the chat that is available on the Kick website. You can send a message as a Bot account or your User account.
    /// </summary>
    public IChat Chat { get; }
    
    /// <summary>
    ///     Channels APIs allow an app to interact with channels in the Kick website. Available data will depend on the scopes attached to the authorization token used.
    /// </summary>
    public IChannels Channels { get; }
    
    /// <summary>
    ///     [WEBHOOKS] Event Subscriptions APIs allow you to subscribe to events on Kick e.g. chat messages, follows, subscriptions.
    ///     Those events are then sent to a webhook URL that you provide.
    /// </summary>
    public IEventSubscriptions EventSubscriptions { get; }
    
    /// <summary>
    ///     Livestreams APIs allow an app to interact with livestreams in the Kick website.
    ///     Available data will depend on the scopes attached to the authorization token used.
    /// </summary>
    public ILivestreams Livestreams { get; }
    
    /// <summary>
    ///     Moderation APIs enable you to restrict user participation in chat rooms through temporary timeouts or permanent bans,
    ///     as well as reverse these actions by removing timeouts or unbanning specific users.
    /// </summary>
    public IModeration Moderation { get; }
    
    /// <summary>
    ///     User APIs allow apps to interact with user information. Scopes will vary and sensitive data will be available to User Access Tokens with the required scopes.
    /// </summary>
    public IUsers Users { get; }
    
    /// <summary>
    ///     The settings for the Kick API (like ClientId, Secrets, etc).
    /// </summary>
    public ApiSettings ApiSettings { get; }
}