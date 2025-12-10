using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks.Payloads;
using Newtonsoft.Json;

namespace KickLib.Webhooks;

/// <summary>
///     Parser for webhook events.
/// </summary>
public static class WebhookEventParser
{
    /// <summary>
    ///     Header name for the Kick event type.
    /// </summary>
    public const string KickEventTypeHeader = "Kick-Event-Type";

    /// <summary>
    ///     Header name for the Kick event version.
    /// </summary>
    public const string KickEventVersionHeader = "Kick-Event-Version";

    /// <summary>
    ///     Header name for the Kick event message timestamp.
    /// </summary>
    public const string KickEventMessageTimestampHeader = "Kick-Event-Message-Timestamp";

    /// <summary>
    ///     Header name for the Kick event signature.
    /// </summary>
    public const string KickEventSignatureHeader = "Kick-Event-Signature";

    /// <summary>
    ///     Header name for the Kick event subscription ID (ULID).
    /// </summary>
    public const string KickEventSubscriptionIdHeader = "Kick-Event-Subscription-Id";

    /// <summary>
    ///     Header name for the Kick event message ID (ULID).
    /// </summary>
    public const string KickEventMessageIdHeader = "Kick-Event-Message-Id";
    
    /// <summary>
    ///     Parse event payload to specific event type.
    /// </summary>
    /// <param name="eventType">Event type to parse as.</param>
    /// <param name="payload">Payload to be parsed.</param>
    /// <returns>Returns <c>true</c> if parse was successful.</returns>
    /// <exception cref="ArgumentException">Throws when parsing unknown type.</exception>
    public static WebhookEventBase Parse(EventType eventType, string payload)
    {
        TryParse(eventType, payload, out var webhookEvent);
        return webhookEvent ?? throw new ArgumentException("Unknown event type");
    }
    
    /// <summary>
    ///     Try parse event payload to specific event type.
    /// </summary>
    /// <param name="eventType">Event type to parse as.</param>
    /// <param name="payload">Payload to be parsed.</param>
    /// <param name="webhookEvent">Parsed object (if successful).</param>
    /// <returns>Returns <c>true</c> if parse was successful.</returns>
    public static bool TryParse(EventType eventType, string payload, out WebhookEventBase? webhookEvent)
    {
        webhookEvent = eventType switch
        {
            EventType.ChannelFollowed => ParseChannelFollowedEvent(payload),
            EventType.ChannelSubscriptionGifts => ParseChannelGiftedSubscriptionEvent(payload),
            EventType.ChannelSubscriptionNew => ParseChannelNewSubscriptionEvent(payload),
            EventType.ChannelSubscriptionRenewal => ParseChannelSubscriptionRenewalEvent(payload),
            EventType.ChannelRewardRedemptionUpdated => ParseChannelRewardRedemptionUpdatedEvent(payload),
            EventType.ChatMessageSent => ParseChatMessageSentEvent(payload),
            EventType.LivestreamStatusUpdated => ParseLivestreamStatusUpdatedEvent(payload),
            EventType.LivestreamMetadataUpdated => ParseLivestreamMetadataUpdatedEvent(payload),
            EventType.ModerationUserBanned => ParseModerationUserBannedEvent(payload),
            EventType.KicksGifted => ParseKicksGiftedEvent(payload),
            _ => null
        };
        
        return webhookEvent != null;
    }
    
    /// <summary>
    ///     Parse chat.message.sent event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ChatMessageSentEvent? ParseChatMessageSentEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChatMessageSentEvent>(payload);
    }
    
    /// <summary>
    ///     Parse channel.subscription.new event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ChannelNewSubscriptionEvent? ParseChannelNewSubscriptionEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelNewSubscriptionEvent>(payload);
    }
    
    /// <summary>
    ///     Parse channel.subscription.gifts event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ChannelGiftedSubscriptionEvent? ParseChannelGiftedSubscriptionEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelGiftedSubscriptionEvent>(payload);
    }
    
    /// <summary>
    ///     Parse channel.subscription.renewal event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ChannelSubscriptionRenewalEvent? ParseChannelSubscriptionRenewalEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelSubscriptionRenewalEvent>(payload);
    }
    
    /// <summary>
    ///     Parse channel.reward.redemption.updated event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ChannelRewardRedemptionUpdatedEvent? ParseChannelRewardRedemptionUpdatedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelRewardRedemptionUpdatedEvent>(payload);
    }
    
    /// <summary>
    ///     Parse channel.followed event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ChannelFollowedEvent? ParseChannelFollowedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelFollowedEvent>(payload);
    }
    
    /// <summary>
    ///     Parse livestream.status.updated event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static LivestreamStatusUpdatedEvent? ParseLivestreamStatusUpdatedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<LivestreamStatusUpdatedEvent>(payload);
    }
    
    /// <summary>
    ///     Parse livestream.metadata.updated event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static LivestreamMetadataUpdatedEvent? ParseLivestreamMetadataUpdatedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<LivestreamMetadataUpdatedEvent>(payload);
    }
    
    /// <summary>
    ///     Parse moderation.banned event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static ModerationUserBannedEvent? ParseModerationUserBannedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ModerationUserBannedEvent>(payload);
    }
    
    /// <summary>
    ///     Parse kicks.gifted event.
    /// </summary>
    /// <param name="payload">Payload to parse.</param>
    /// <returns>Returns parsed event or <c>null</c>.</returns>
    public static KicksGiftedEvent? ParseKicksGiftedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<KicksGiftedEvent>(payload);
    }
}