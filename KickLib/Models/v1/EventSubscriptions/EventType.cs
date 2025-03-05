namespace KickLib.Models.v1.EventSubscriptions;

/// <summary>
///     A list of event types that can be subscribed to for Webhook notifications.
/// </summary>
public enum EventType
{
    /// <summary>
    ///     Event type chat.message.sent.
    /// </summary>
    ChatMessageSent,
    
    /// <summary>
    ///     Event type channel.followed.
    /// </summary>
    ChannelFollowed,
    
    /// <summary>
    ///     Event type channel.subscription.renewal.
    /// </summary>
    ChannelSubscriptionRenewal,
    
    /// <summary>
    ///     Event type channel.subscription.gifts.
    /// </summary>
    ChannelSubscriptionGifts,
    
    /// <summary>
    ///     Event type channel.subscription.new.
    /// </summary>
    ChannelSubscriptionNew
}