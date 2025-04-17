using FluentAssertions;
using KickLib.Webhooks;
using KickLib.Webhooks.Payloads;

namespace KickLib.Tests;

public class EventPayloadsTests : BaseKickLibTests
{
    public EventPayloadsTests() : base("Data.WebhookPayloads")
    {
    }
    
    [Theory]
    [InlineData("ChatMessageSentEventPayload")]
    [InlineData("ChatMessageSentEventPayload_NoEmotes")]
    public void CorrectlyDeserialize_ChatMessageSentEventPayload(string data)
    {
        var payload = GetPayload(data);

        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }
    
    [Fact]
    public void ChatMessageSentEventPayload_ContainsCorrectValues()
    {
        var payload = GetPayload("ChatMessageSentEventPayload");

        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.Sender.Identity.Should().NotBeNull();
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.Moderator);
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.Subscriber);
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.SubGifter);
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.SubGifter && x.Count == 5);
    }
    
    [Fact]
    public void CorrectlyDeserialize_ChannelFollowedEventPayload()
    {
        var payload = GetPayload("ChannelFollowedEventPayload");

        var webhookEvent = WebhookEventParser.ParseChannelFollowedEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }

    [Theory]
    [InlineData("ChannelGiftedSubscriptionEventPayload")]
    [InlineData("ChannelGiftedSubscriptionEventPayload_Anonymous")]
    public void CorrectlyDeserialize_ChannelGiftedSubscriptionEventPayload(string data)
    {
        var payload = GetPayload(data);

        var webhookEvent = WebhookEventParser.ParseChannelGiftedSubscriptionEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }

    [Fact]
    public void CorrectlyDeserialize_ChannelNewSubscriptionEventPayload()
    {
        var payload = GetPayload("ChannelNewSubscriptionEventPayload");

        var webhookEvent = WebhookEventParser.ParseChannelNewSubscriptionEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }

    [Fact]
    public void CorrectlyDeserialize_ChannelSubscriptionRenewalEventPayload()
    {
        var payload = GetPayload("ChannelSubscriptionRenewalEventPayload");

        var webhookEvent = WebhookEventParser.ParseChannelSubscriptionRenewalEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }
    
    [Fact]
    public void CorrectlyDeserialize_LivestreamMetadataUpdatedEventPayload()
    {
        var payload = GetPayload("LivestreamMetadataUpdatedEventPayload");

        var webhookEvent = WebhookEventParser.ParseLivestreamMetadataUpdatedEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }

    [Theory]
    [InlineData("LivestreamStatusUpdatedEventPayload_Live")]
    [InlineData("LivestreamStatusUpdatedEventPayload_Offline")]
    public void CorrectlyDeserialize_LivestreamStatusUpdatedEventPayload(string data)
    {
        var payload = GetPayload(data);

        var webhookEvent = WebhookEventParser.ParseLivestreamStatusUpdatedEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
    }
}