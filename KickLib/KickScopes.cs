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
    ///     Read channel rewards scope.
    /// </summary>
    /// <remarks>
    ///     Read Channel points rewards information on a channel
    /// </remarks>
    public const string ChannelRewardsRead = "channel:rewards:read";
    
    /// <summary>
    ///     Update channel rewards scope.
    /// </summary>
    /// <remarks>
    ///     Read, add, edit and delete Channel points rewards on a channel
    /// </remarks>
    public const string ChannelRewardsWrite ="channel:rewards:write";
    
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
    ///     Ban/Unban users scope.
    /// </summary>
    /// <remarks>
    ///     Execute moderation actions for moderators.
    /// </remarks>
    public const string ModerationBan = "moderation:ban";
    
    /// <summary>
    ///     Manage chat messages scope.
    /// </summary>
    /// <remarks>
    ///     Execute moderation actions on chat messages.
    /// </remarks>
    public const string ModerationChatMessageManage = "moderation:chat_message:manage";
    
    /// <summary>
    ///     View KICKs info scope.
    /// </summary>
    /// <remarks>
    ///     View KICKs related information in Kick e.g leaderboards, etc.
    /// </remarks>
    public const string KicksRead = "kicks:read";
}