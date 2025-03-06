using FluentAssertions;
using KickLib.Extensions;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks;

namespace KickLib.Tests;

public class WebhookExtensionsTests
{
    [Theory]
    [InlineData(EventType.ChannelFollowed, WebhookEventTypes.ChannelFollowed)]
    [InlineData(EventType.ChatMessageSent, WebhookEventTypes.ChatMessageSent)]
    [InlineData(EventType.ChannelSubscriptionRenewal, WebhookEventTypes.ChannelSubscriptionRenewal)]
    [InlineData(EventType.ChannelSubscriptionGifts, WebhookEventTypes.ChannelGiftedSubscription)]
    [InlineData(EventType.ChannelSubscriptionNew, WebhookEventTypes.ChannelNewSubscription)]
    [InlineData(EventType.LivestreamStatusUpdated, WebhookEventTypes.LivestreamStatusUpdated)]
    public void CorrectlyExtractEventType(EventType eventType, string expected)
    {
        var eventName = eventType.GetEventName();

        eventName.Should().Be(expected);
    }
    
    [Theory]
    [InlineData(WebhookEventTypes.ChannelFollowed, "1", EventType.ChannelFollowed, 1)]
    [InlineData(WebhookEventTypes.ChatMessageSent, "1", EventType.ChatMessageSent, 1)]
    [InlineData(WebhookEventTypes.ChannelSubscriptionRenewal, "1", EventType.ChannelSubscriptionRenewal, 1)]
    [InlineData(WebhookEventTypes.ChannelGiftedSubscription, "1", EventType.ChannelSubscriptionGifts, 1)]
    [InlineData(WebhookEventTypes.ChannelNewSubscription, "1", EventType.ChannelSubscriptionNew, 1)]
    [InlineData(WebhookEventTypes.LivestreamStatusUpdated, "1", EventType.LivestreamStatusUpdated, 1)]
    public void GetEventType_ReturnsCorrectValue(string headerType, string version, EventType expectedType, int expectedVersion)
    {
        var headers = new Dictionary<string, string>
        {
            { WebhookEventParser.KickEventTypeHeader, headerType },
            { WebhookEventParser.KickEventVersionHeader, version }
        };
        
        var eventType = headers.GetEventType();
        
        eventType.Should().NotBeNull();
        eventType!.Value.EventType.Should().Be(expectedType);
        eventType!.Value.Version.Should().Be(expectedVersion);
    }
    
    [Fact]
    public void GetEventType_ReturnsNull_WhenHeadersAreNull()
    {
        var eventType = ((Dictionary<string, string>?)null).GetEventType();
        
        eventType.Should().BeNull();
    }
    
    [Fact]
    public void GetEventType_ReturnsNull_WhenEventTypeHeaderIsMissing()
    {
        var headers = new Dictionary<string, string>
        {
            { WebhookEventParser.KickEventVersionHeader, "1" }
        };
        
        var eventType = headers.GetEventType();
        
        eventType.Should().BeNull();
    }

    [Fact]
    public void GetWebhookEventInfo_ReturnsCorrectValue()
    {
        var headers = new Dictionary<string, string>
        {
            { WebhookEventParser.KickEventTypeHeader, WebhookEventTypes.ChannelFollowed },
            { WebhookEventParser.KickEventVersionHeader, "1" },
            { WebhookEventParser.KickEventMessageTimestampHeader, "2023-10-01T12:34:56Z" },
            { WebhookEventParser.KickEventSignatureHeader, "signature" },
            { WebhookEventParser.KickEventSubscriptionIdHeader, "subscriptionId" },
            { WebhookEventParser.KickEventMessageIdHeader, "messageId" }
        };

        var eventInfo = headers.GetWebhookEventInfo();

        eventInfo.Should().NotBeNull();
        eventInfo!.EventType.Should().Be(WebhookEventTypes.ChannelFollowed);
        eventInfo.EventVersion.Should().Be(1);
        eventInfo.MessageTimestamp.Should().Be("2023-10-01T12:34:56Z");
        eventInfo.Signature.Should().Be("signature");
        eventInfo.SubscriptionId.Should().Be("subscriptionId");
        eventInfo.MessageId.Should().Be("messageId");
    }

    [Fact]
    public void GetWebhookEventInfo_ReturnsNull_WhenHeadersAreNull()
    {
        var eventInfo = ((Dictionary<string, string>?)null).GetWebhookEventInfo();

        eventInfo.Should().BeNull();
    }

    [Fact]
    public void GetWebhookEventInfo_ReturnsNull_WhenAnyHeaderIsMissing()
    {
        var headers = new Dictionary<string, string>
        {
            { WebhookEventParser.KickEventTypeHeader, WebhookEventTypes.ChannelFollowed },
            { WebhookEventParser.KickEventVersionHeader, "1" },
            { WebhookEventParser.KickEventMessageTimestampHeader, "2023-10-01T12:34:56Z" },
            { WebhookEventParser.KickEventSignatureHeader, "signature" },
            { WebhookEventParser.KickEventSubscriptionIdHeader, "subscriptionId" }
        };

        var eventInfo = headers.GetWebhookEventInfo();

        eventInfo.Should().BeNull();
    }
}