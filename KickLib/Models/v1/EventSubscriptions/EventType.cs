namespace KickLib.Models.v1.EventSubscriptions;

/// <summary>
///     A list of event types that can be subscribed to for Webhook notifications.
/// </summary>
public enum EventType
{
    /// <summary>
    ///     Unknown is NOT a valid Kick event type.
    ///     Unknown is used to represent an event type that is not recognized by the SDK.
    /// </summary>
    Unknown,
    
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
    ChannelSubscriptionNew,
    
	/// <summary>
	///     Event type channel.reward.redemption.updated.
	/// </summary>
	ChannelRewardRedemptionUpdated,

    /// <summary>
    ///     Event type livestream.status.updated.
    /// </summary>
    LivestreamStatusUpdated,
    
    /// <summary>
    ///     Event type livestream.metadata.updated.
    /// </summary>
    LivestreamMetadataUpdated,
    
    /// <summary>
    ///     Event type moderation.banned.
    /// </summary>
    ModerationUserBanned,
    
    /// <summary>
    ///     Event type kicks.gifted.
    /// </summary>
    KicksGifted
}