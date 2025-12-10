namespace KickLib.Webhooks;

/// <summary>
///     Contains the event types that can be received from a webhook.
/// </summary>
public static class WebhookEventTypes
{
    /// <summary>
    ///     Event type for a chat message being sent.
    /// </summary>
    public const string ChatMessageSent = "chat.message.sent";

    /// <summary>
    ///     Event type for a channel being followed.
    /// </summary>
    public const string ChannelFollowed = "channel.followed";

    /// <summary>
    ///     Event type for a channel subscription being renewed.
    /// </summary>
    public const string ChannelSubscriptionRenewal = "channel.subscription.renewal";

    /// <summary>
    ///     Event type for a gifted subscription in a channel.
    /// </summary>
    public const string ChannelGiftedSubscription = "channel.subscription.gifts";

    /// <summary>
    ///     Event type for a new subscription to a channel.
    /// </summary>
    public const string ChannelNewSubscription = "channel.subscription.new";
    
    /// <summary>
    ///     Event type channel.reward.redemption.updated.
    /// </summary>
    public const string ChannelRewardRedemptionUpdated = "channel.reward.redemption.updated";

    /// <summary>
    ///     Event type for a livestream status update.
    /// </summary>
    public const string LivestreamStatusUpdated = "livestream.status.updated";

    /// <summary>
    ///     Event type for a livestream metadata update.
    /// </summary>
    public const string LivestreamMetadataUpdated = "livestream.metadata.updated";
    
    /// <summary>
    ///     Event type for a moderation when user is banned (or timed-out).
    /// </summary>
    public const string ModerationUserBanned = "moderation.banned";
    
    /// <summary>
    ///     Event type for a gifted kicks to a channel.
    /// </summary>
    public const string KicksGifted = "kicks.gifted";
}