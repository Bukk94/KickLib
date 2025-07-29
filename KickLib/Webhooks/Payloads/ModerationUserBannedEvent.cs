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
    public KickUser Moderator { get; set; } = new();
    
    /// <summary>
    ///     Details about the user who was banned or timed-out.
    /// </summary>
    [JsonProperty(PropertyName = "banned_user")]
    public KickUser BannedUser { get; set; } = new();

    /// <summary>
    ///     Ban metadata.
    /// </summary>
    public BanMetadata Metadata { get; set; } = new();
}