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
}