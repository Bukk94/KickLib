namespace KickLib.Webhooks.Payloads;

public class ChannelFollowedEvent : WebhookEventBase
{
    public required KickUser Follower { get; set; }
}