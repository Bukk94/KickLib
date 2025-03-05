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
            "chat.message.sent" => (EventType.ChatMessageSent, version),
            "channel.followed" => (EventType.ChannelFollowed, version),
            "channel.subscription.renewal" => (EventType.ChannelSubscriptionRenewal, version),
            "channel.subscription.gifts" => (EventType.ChannelSubscriptionGifts, version),
            "channel.subscription.new" => (EventType.ChannelSubscriptionNew, version),
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
            EventType.ChatMessageSent => "chat.message.sent",
            EventType.ChannelFollowed => "channel.followed",
            EventType.ChannelSubscriptionRenewal => "channel.subscription.renewal",
            EventType.ChannelSubscriptionGifts => "channel.subscription.gifts",
            EventType.ChannelSubscriptionNew => "channel.subscription.new",
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
            DateTimeOffset.Parse(messageTimestamp),
            eventSignature,
            subscriptionId,
            messageId
        );
    }
}
