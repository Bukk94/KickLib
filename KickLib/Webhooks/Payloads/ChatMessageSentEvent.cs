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
    public string MessageId { get; set; } = string.Empty;

    /// <summary>
    ///     If user is replying to another message, details about that message.
    /// </summary>
    [JsonProperty(PropertyName = "replies_to")]
    public ReplyToDetails? RepliesTo { get; set; }
    
    /// <summary>
    ///     Details of the user who sent the message.
    /// </summary>
    public KickUser Sender { get; set; } = new();
    
    /// <summary>
    ///     The content of the message.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    ///     Information about the emotes present in the message.
    /// </summary>
    public ICollection<Emotes>? Emotes { get; set; }
    
    /// <summary>
    ///     When chat message was sent.
    /// </summary>
    [JsonProperty(PropertyName = "created_at")]
    public DateTimeOffset CreatedAt { get; set; }
}