using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a channel subscription renewal event.
/// </summary>
public class ChannelSubscriptionRenewalEvent : WebhookEventBase
{
    /// <summary>
    ///     The user who renewed the subscription.
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