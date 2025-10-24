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
    [InlineData("ChatMessageSentEventWithReplyPayload")]
    [InlineData("ChatMessageSentEventPayload_NoEmotes")]
    public void CorrectlyDeserialize_ChatMessageSentEventPayload(string data)
    {
        var payload = GetPayload(data);

        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.MessageId.Should().NotBeNullOrEmpty();
        webhookEvent.Broadcaster.Should().NotBeNull();
        webhookEvent.Sender.Should().NotBeNull();
        webhookEvent.Content.Should().NotBeNullOrEmpty();
    }
    
    [Fact]
    public void ChatMessageSentEventPayload_ContainsCorrectValues()
    {
        var payload = GetPayload("ChatMessageSentEventPayload");

        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.CreatedAt.Should().NotBe(DateTimeOffset.MinValue);
        webhookEvent.Sender.Identity.Should().NotBeNull();
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.Moderator);
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.Subscriber);
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.SubGifter);
        webhookEvent.Sender.Identity.Badges.Should().Contain(x => x.Type == BadgeType.SubGifter && x.Count == 5);
    }
    
    [Fact]
    public void ChatMessageSentEventWithReplyPayload_ContainsCorrectValues()
    {
        var payload = GetPayload("ChatMessageSentEventWithReplyPayload");

        var webhookEvent = WebhookEventParser.ParseChatMessageSentEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.RepliesTo.Should().NotBeNull();
        webhookEvent.RepliesTo.MessageId.Should().Be("unique_message_id_456");
        webhookEvent.RepliesTo.Content.Should().Be("This is the parent message!");
        webhookEvent.RepliesTo.Sender.Should().NotBeNull();
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
        webhookEvent.Broadcaster.Should().NotBeNull();
        webhookEvent.Gifter.Should().NotBeNull();
        webhookEvent.Subscriber.Should().NotBeNull();
        webhookEvent.Subscriber.UserId.Should().Be(987654321);
        webhookEvent.Duration.Should().Be(1);
        webhookEvent.CreatedAt.Should().BeCloseTo(DateTimeOffset.Parse("2025-01-14T16:08:06Z"), TimeSpan.FromSeconds(1));
        webhookEvent.ExpiresAt.Should().BeCloseTo(DateTimeOffset.Parse("2025-03-15T16:08:06Z"), TimeSpan.FromSeconds(1));
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
    
    [Theory]
    [InlineData("ModerationUserBannedEventPayload")]
    [InlineData("ModerationUserTimedOutEventPayload")]
    public void CorrectlyDeserialize_ModerationUserBannedEventPayload(string data)
    {
        var payload = GetPayload(data);

        var webhookEvent = WebhookEventParser.ParseModerationUserBannedEvent(payload);
        
        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.Moderator.Should().NotBeNull();
        webhookEvent.BannedUser.Should().NotBeNull();
        webhookEvent.Metadata.Should().NotBeNull();
        webhookEvent.Metadata.Reason.Should().NotBeNull();
    }
    
    [Fact]
    public void CorrectlyDeserialize_KicksGiftedEventPayload()
    {
        var payload = GetPayload("KicksGiftedEventPayload");

        var webhookEvent = WebhookEventParser.ParseKicksGiftedEvent(payload);

        payload.Should().NotBeNull();
        webhookEvent.Should().NotBeNull();
        webhookEvent.Broadcaster.Should().NotBeNull();
        webhookEvent.Broadcaster.Username.Should().Be("broadcaster_name");
        webhookEvent.Broadcaster.ChannelSlug.Should().Be("broadcaster_channel");
        webhookEvent.Broadcaster.ProfilePicture.Should().Be("https://example.com/broadcaster_avatar.jpg");
        webhookEvent.Broadcaster.IsVerified.Should().BeTrue();
        webhookEvent.Broadcaster.UserId.Should().Be(123456789);
        webhookEvent.Sender.Should().NotBeNull();
        webhookEvent.Sender.IsVerified.Should().BeFalse();
        webhookEvent.Sender.Username.Should().Be("gift_sender");
        webhookEvent.Sender.ChannelSlug.Should().Be("gift_sender_channel");
        webhookEvent.Sender.ProfilePicture.Should().Be("https://example.com/sender_avatar.jpg");
        webhookEvent.Sender.UserId.Should().Be(987654321);
        webhookEvent.Gift.Amount.Should().Be(100);
        webhookEvent.Gift.Name.Should().Be("Full Send");
        webhookEvent.Gift.Type.Should().Be("BASIC");
        webhookEvent.Gift.Tier.Should().Be("BASIC");
        webhookEvent.Gift.Message.Should().Be("w");
    }
}