namespace KickLib;

/// <summary>
///     Defines available Kick API scopes.
/// </summary>
public static class KickScopes
{
    /// <summary>
    ///     Read user info scope.
    /// </summary>
    /// <remarks>
    ///     View user information in Kick including username, streamer ID, etc.
    /// </remarks>
    public const string UserRead = "user:read";
    
    /// <summary>
    ///     Read channel info scope.
    /// </summary>
    /// <remarks>
    ///     View channel information in Kick including channel description, category, etc.
    /// </remarks>
    public const string ChannelRead = "channel:read";
    
    /// <summary>
    ///     Update channel info scope.
    /// </summary>
    /// <remarks>
    ///     Update livestream metadata for a channel based on the channel ID.
    /// </remarks>
    public const string ChannelWrite = "channel:write";
    
    /// <summary>
    ///     Write to chat scope.
    /// </summary>
    /// <remarks>
    ///     Send chat messages and allow chat bots to post in your chat.
    /// </remarks>
    public const string ChatWrite = "chat:write";
    
    /// <summary>
    ///     Read stream key scope. [SENSITIVE]
    /// </summary>
    /// <remarks>
    ///     Read a user's stream URL and stream key.
    /// </remarks>
    public const string StreamKeyRead = "streamkey:read";
    
    /// <summary>
    ///     Subscribe to events scope.
    /// </summary>
    /// <remarks>
    ///     Subscribe to all channel events on Kick e.g. chat messages, follows, subscriptions.
    /// </remarks>
    public const string EventsSubscribe = "events:subscribe";
    
    /// <summary>
    ///     Execute moderation actions for moderators.
    /// </summary>
    public const string ModerationBan = "moderation:ban";
}