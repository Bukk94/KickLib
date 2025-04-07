using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a channel gifted subscription event.
/// </summary>
public class ChannelGiftedSubscriptionEvent : WebhookEventBase
{
    /// <summary>
    ///     The user who gifted the subscription.
    /// </summary>
    public required KickUser Gifter { get; set; }
    
    /// <summary>
    ///     Users who received the gifted subscription.
    /// </summary>
    public required ICollection<KickUser> Giftees { get; set; }

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