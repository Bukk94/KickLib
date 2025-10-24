using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents an event when a user gifts kicks to a channel.
/// </summary>
public class KicksGiftedEvent : WebhookEventBase
{
    /// <summary>
    ///     Details about the user, who gifted the kicks.
    /// </summary>
    public KickUser Sender { get; set; } = new();
    
    /// <summary>
    ///     Details about the gifted kick.
    /// </summary>
    public GiftAmount Gift { get; set; } = new();
    
    /// <summary>
    ///     When chat message was sent.
    /// </summary>
    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}