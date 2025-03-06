using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks.Payloads;
using Newtonsoft.Json;

namespace KickLib.Webhooks;

public static class WebhookEventParser
{
    public const string KickEventTypeHeader = "Kick-Event-Type";
    public const string KickEventVersionHeader = "Kick-Event-Version";
    public const string KickEventMessageTimestampHeader = "Kick-Event-Message-Timestamp";
    public const string KickEventSignatureHeader = "Kick-Event-Signature";
    public const string KickEventSubscriptionIdHeader = "Kick-Event-Subscription-Id";
    public const string KickEventMessageIdHeader = "Kick-Event-Message-Id";
    
    public static WebhookEventBase Parse(EventType eventType, string payload)
    {
        TryParse(eventType, payload, out var webhookEvent);
        return webhookEvent ?? throw new ArgumentException("Unknown event type");
    }
    
    public static bool TryParse(EventType eventType, string payload, out WebhookEventBase? webhookEvent)
    {
        webhookEvent = eventType switch
        {
            EventType.ChannelFollowed => ParseChannelFollowedEvent(payload),
            EventType.ChannelSubscriptionGifts => ParseChannelGiftedSubscriptionEvent(payload),
            EventType.ChannelSubscriptionNew => ParseChannelNewSubscriptionEvent(payload),
            EventType.ChannelSubscriptionRenewal => ParseChannelSubscriptionRenewalEvent(payload),
            EventType.ChatMessageSent => ParseChatMessageSentEvent(payload),
            EventType.LivestreamStatusUpdated => ParseLivestreamStatusUpdatedEvent(payload),
            _ => null
        };
        
        return webhookEvent != null;
    }
    
    public static ChatMessageSentEvent? ParseChatMessageSentEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChatMessageSentEvent>(payload);
    }
    
    public static ChannelNewSubscriptionEvent? ParseChannelNewSubscriptionEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelNewSubscriptionEvent>(payload);
    }
    
    public static ChannelGiftedSubscriptionEvent? ParseChannelGiftedSubscriptionEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelGiftedSubscriptionEvent>(payload);
    }
    
    public static ChannelSubscriptionRenewalEvent? ParseChannelSubscriptionRenewalEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelSubscriptionRenewalEvent>(payload);
    }
    
    public static ChannelFollowedEvent? ParseChannelFollowedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<ChannelFollowedEvent>(payload);
    }
    
    public static LivestreamStatusUpdatedEvent? ParseLivestreamStatusUpdatedEvent(string payload)
    {
        return JsonConvert.DeserializeObject<LivestreamStatusUpdatedEvent>(payload);
    }
}