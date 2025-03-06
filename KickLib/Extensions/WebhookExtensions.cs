using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks;

namespace KickLib.Extensions;

public static class WebhookExtensions
{
    /// <summary>
    ///     Extracts the event type and version from the headers of a request.
    /// </summary>
    /// <param name="headers">A key-value pair headers of the incoming request (e.g. HttpContext.Request).</param>
    public static (EventType EventType, int Version)? GetEventType(this Dictionary<string, string>? headers)
    {
        if (headers is null ||
            !headers.TryGetValue(WebhookEventParser.KickEventTypeHeader, out var type) ||
            !headers.TryGetValue(WebhookEventParser.KickEventVersionHeader, out var versionValue) ||
            !int.TryParse(versionValue, out var version))
        {
            return null;
        }

        return type switch
        {
            WebhookEventTypes.ChatMessageSent => (EventType.ChatMessageSent, version),
            WebhookEventTypes.ChannelFollowed => (EventType.ChannelFollowed, version),
            WebhookEventTypes.ChannelSubscriptionRenewal => (EventType.ChannelSubscriptionRenewal, version),
            WebhookEventTypes.ChannelGiftedSubscription => (EventType.ChannelSubscriptionGifts, version),
            WebhookEventTypes.ChannelNewSubscription => (EventType.ChannelSubscriptionNew, version),
            WebhookEventTypes.LivestreamStatusUpdated => (EventType.LivestreamStatusUpdated, version),
            _ => null
        };
    }

    /// <summary>
    ///     Get event name from event type.
    /// </summary>
    public static string GetEventName(this EventType type)
    {
        return type switch
        {
            EventType.ChatMessageSent => WebhookEventTypes.ChatMessageSent,
            EventType.ChannelFollowed => WebhookEventTypes.ChannelFollowed,
            EventType.ChannelSubscriptionRenewal => WebhookEventTypes.ChannelSubscriptionRenewal,
            EventType.ChannelSubscriptionGifts => WebhookEventTypes.ChannelGiftedSubscription,
            EventType.ChannelSubscriptionNew => WebhookEventTypes.ChannelNewSubscription,
            EventType.LivestreamStatusUpdated => WebhookEventTypes.LivestreamStatusUpdated,
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    /// <summary>
    ///     Extracts event information from the headers of a request.
    /// </summary>
    /// <param name="headers">A key-value pair headers of the incoming request (e.g. HttpContext.Request).</param>
    public static WebhookEventInfo? GetWebhookEventInfo(this Dictionary<string, string>? headers)
    {
        if (headers is null)
        {
            return null;
        }
        
        var eventType = headers.GetValueOrDefault(WebhookEventParser.KickEventTypeHeader);
        var version = headers.GetValueOrDefault(WebhookEventParser.KickEventVersionHeader);
        var messageTimestamp = headers.GetValueOrDefault(WebhookEventParser.KickEventMessageTimestampHeader);
        var eventSignature = headers.GetValueOrDefault(WebhookEventParser.KickEventSignatureHeader);
        var subscriptionId = headers.GetValueOrDefault(WebhookEventParser.KickEventSubscriptionIdHeader);
        var messageId = headers.GetValueOrDefault(WebhookEventParser.KickEventMessageIdHeader);
        
        // If any of the fields are null, return null
        if (eventType == null || version == null || messageTimestamp == null || eventSignature == null || subscriptionId == null || messageId == null)
        {
            return null;
        }
        
        return new WebhookEventInfo
        (
            eventType,
            int.Parse(version),
            messageTimestamp,
            eventSignature,
            subscriptionId,
            messageId
        );
    }
}
