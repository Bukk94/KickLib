namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a channel followed event.
/// </summary>
public class ChannelFollowedEvent : WebhookEventBase
{
    /// <summary>
    ///     The user who followed the channel.
    /// </summary>
    public required KickUser Follower { get; set; }
}