using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a new subscription event.
/// </summary>
public class ChannelNewSubscriptionEvent : WebhookEventBase
{
    /// <summary>
    ///     The user who subscribed to the channel (same value as Subscriber).
    /// </summary>
    [Obsolete("Use Subscriber property instead.")]
    public KickUser Gifter => Subscriber;
    
    /// <summary>
    ///     The user who subscribed to the channel.
    /// </summary>
    public KickUser Subscriber { get; set; } = new();
    
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
