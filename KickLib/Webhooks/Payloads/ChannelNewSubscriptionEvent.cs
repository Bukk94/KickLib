using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a new subscription event.
/// </summary>
public class ChannelNewSubscriptionEvent : WebhookEventBase
{
    /// <summary>
    ///     The user who subscribed to the channel.
    /// </summary>
    public required KickUser Gifter { get; set; }

    /// <summary>
    ///     The duration of the subscription in months.
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    ///     When the subscription was created.
    /// </summary>
    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    ///     When the subscription expires.
    /// </summary>
    [JsonProperty(PropertyName = "expires_at")]
    public DateTimeOffset ExpiresAt { get; set; }
}