using FluentAssertions;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks;
using KickLib.Webhooks.Payloads;

namespace KickLib.Tests;

public class EventParserTests : BaseKickLibTests
{
    public EventParserTests() : base("Data.WebhookPayloads")
    {
    }
    
    public static IEnumerable<object[]> ParserInputData =>
        new List<object[]>
        {
            new object[] { "ChatMessageSentEventPayload", EventType.ChatMessageSent, typeof(ChatMessageSentEvent) },
            new object[] { "ChannelFollowedEventPayload", EventType.ChannelFollowed, typeof(ChannelFollowedEvent) },
            new object[] { "ChannelGiftedSubscriptionEventPayload", EventType.ChannelSubscriptionGifts, typeof(ChannelGiftedSubscriptionEvent) },
            new object[] { "ChannelNewSubscriptionEventPayload", EventType.ChannelSubscriptionNew, typeof(ChannelNewSubscriptionEvent) },
            new object[] { "ChannelSubscriptionRenewalEventPayload", EventType.ChannelSubscriptionRenewal, typeof(ChannelSubscriptionRenewalEvent) },
            new object[] { "LivestreamStatusUpdatedEventPayload_Live", EventType.LivestreamStatusUpdated, typeof(LivestreamStatusUpdatedEvent) }
        };
    
    [Fact]
    public void WebhookEventParser_ParseChatPayload()
    {
        var payload = GetPayload("ChatMessageSentEventPayload");
        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);
        
        webhookEvent.Should().NotBeNull();
        webhookEvent.MessageId.Should().NotBeNull();
        webhookEvent.Broadcaster.Should().NotBeNull();
        webhookEvent.Sender.Should().NotBeNull();
        webhookEvent.Content.Should().NotBeNull();
    }
    
    [Theory]
    [MemberData(nameof(ParserInputData))]
    public void WebhookEventParser_ParseCorrectType(string payloadResource, EventType eventType, Type eventObjectType)
    {
        var payload = GetPayload(payloadResource);

        var webhookEvent = WebhookEventParser.Parse(eventType, payload);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.Should().BeOfType(eventObjectType);
    }
    
    [Theory]
    [MemberData(nameof(ParserInputData))]
    public void WebhookEventParser_TryParse_ReturnsCorrectValue(string payloadResource, EventType eventType, Type eventObjectType)
    {
        var payload = GetPayload(payloadResource);

        var success = WebhookEventParser.TryParse(eventType, payload, out var webhookEvent);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        success.Should().BeTrue();
        webhookEvent.Should().BeOfType(eventObjectType);
    }

    [Fact]
    public void WebhookEventInfo_ValidateKickSignature()
    {
        var payload = GetPayload("ValidationPayload");

        const string signature = "fpZCxfE8lojfMhDPvSpmEjHbJH4+6OFVSLStKgiTxH7QXQw/M3sdWWl0o/pxBz0vA9xXP8x3l+z7WNkT3C+6K7MkEZBtvv+88IAgWyJ2uTLKJtuFn5FIIQKTv1tAqOeFIp1A56DJR9eJ/yzG+flj9RwSNcvMPXBHS3X5jisBiKhYrqUUAW6HYuYKMq5cTcxb1IX0hyN5jEkFv2BuWAIlriyVztdXBX1aHENBxCSf1qbFzQ26VCaZNCOGPpLS+4kHzuU8Zkju+o4nAUm+DIC8c1CjYfPIwu/tZb2HPGklXt1ZMQXpnP+F/Oo+NaW8Z0fBl1ZG8wanIVjPClkoDR4QZQ==";
        const string subscriptionId = "01JQ79DGGK8C9117GJN8EHCYGG";
        const string messageId = "01JQR5KV0QC94HMETWYNBWRW4Z";
        const string timestamp = "2025-04-01T07:56:19Z";
        
        var eventInfo = new WebhookEventInfo(
            WebhookEventTypes.LivestreamStatusUpdated,
            1,
            timestamp,
            signature,
            subscriptionId,
            messageId
        );

        eventInfo.ValidateSender(payload).Should().BeTrue();
    }
}