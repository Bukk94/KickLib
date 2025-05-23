using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a moderation event when user was banned or timed-out.
/// </summary>
public class ModerationUserBannedEvent : WebhookEventBase
{
    /// <summary>
    ///     Details about the moderator who performed the action.
    /// </summary>
    public required KickUser Moderator { get; set; }
    
    /// <summary>
    ///     Details about the user who was banned or timed-out.
    /// </summary>
    [JsonProperty(PropertyName = "banned_user")]
    public required KickUser BannedUser { get; set; }

    /// <summary>
    ///     Ban metadata.
    /// </summary>
    public required BanMetadata Metadata { get; set; }
}