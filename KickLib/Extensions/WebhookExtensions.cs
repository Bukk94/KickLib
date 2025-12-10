using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks;

namespace KickLib.Extensions;

/// <summary>
///     Provides extension methods for working with webhook payloads and types.
/// </summary>
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

        var eventType = ToEventType(type);
        
        return eventType != EventType.Unknown
            ? (eventType, version)
            : null;
    }

    /// <summary>
    ///     Converts event type name to EventType.
    /// </summary>
    /// <param name="eventTypeName">Input event type from headers.</param>
    /// <returns>Returns parsed EventType or Unknown if event is not recognized.</returns>
    public static EventType ToEventType(this string eventTypeName)
    {
        return eventTypeName switch
        {
            WebhookEventTypes.ChatMessageSent => EventType.ChatMessageSent,
            WebhookEventTypes.ChannelFollowed => EventType.ChannelFollowed,
            WebhookEventTypes.ChannelSubscriptionRenewal => EventType.ChannelSubscriptionRenewal, 
            WebhookEventTypes.ChannelGiftedSubscription => EventType.ChannelSubscriptionGifts,
            WebhookEventTypes.ChannelNewSubscription => EventType.ChannelSubscriptionNew,
            WebhookEventTypes.ChannelRewardRedemptionUpdated => EventType.ChannelRewardRedemptionUpdated,
            WebhookEventTypes.LivestreamStatusUpdated => EventType.LivestreamStatusUpdated,
            WebhookEventTypes.LivestreamMetadataUpdated => EventType.LivestreamMetadataUpdated,
            WebhookEventTypes.ModerationUserBanned => EventType.ModerationUserBanned,
            WebhookEventTypes.KicksGifted => EventType.KicksGifted,
            _ => EventType.Unknown
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
            EventType.ChannelRewardRedemptionUpdated => WebhookEventTypes.ChannelRewardRedemptionUpdated,
            EventType.LivestreamStatusUpdated => WebhookEventTypes.LivestreamStatusUpdated,
            EventType.LivestreamMetadataUpdated => WebhookEventTypes.LivestreamMetadataUpdated,
            EventType.ModerationUserBanned => WebhookEventTypes.ModerationUserBanned,
            EventType.KicksGifted => WebhookEventTypes.KicksGifted,
            EventType.Unknown => throw new ArgumentException("Unknown event type", nameof(type)),
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
