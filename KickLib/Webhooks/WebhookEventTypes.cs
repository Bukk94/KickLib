namespace KickLib.Webhooks;

/// <summary>
///     Contains the event types that can be received from a webhook.
/// </summary>
public static class WebhookEventTypes
{
    public const string ChatMessageSent = "chat.message.sent";
    public const string ChannelFollowed = "channel.followed";
    public const string ChannelSubscriptionRenewal = "channel.subscription.renewal";
    public const string ChannelGiftedSubscription = "channel.subscription.gifts";
    public const string ChannelNewSubscription = "channel.subscription.new";
    public const string LivestreamStatusUpdated = "livestream.status.updated";
    public const string LivestreamMetadataUpdated = "livestream.metadata.updated";
}