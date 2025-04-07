using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a chat message sent event.
/// </summary>
public class ChatMessageSentEvent : WebhookEventBase
{
    /// <summary>
    ///     ID of the message.
    /// </summary>
    [JsonProperty(PropertyName = "message_id")]
    public required string MessageId { get; set; }
    
    /// <summary>
    ///     Details of the user who sent the message.
    /// </summary>
    public required KickUser Sender { get; set; }
    
    /// <summary>
    ///     The content of the message.
    /// </summary>
    public required string Content { get; set; }
    
    /// <summary>
    ///     Information about the emotes present in the message.
    /// </summary>
    public ICollection<Emotes>? Emotes { get; set; }
}