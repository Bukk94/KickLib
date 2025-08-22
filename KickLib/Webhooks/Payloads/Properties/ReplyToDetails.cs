using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Details about the original message this message is replying to.
/// </summary>
public class ReplyToDetails
{
    /// <summary>
    ///     ID of the message.
    /// </summary>
    [JsonProperty(PropertyName = "message_id")]
    public string MessageId { get; set; } = string.Empty;
    
    /// <summary>
    ///     The content of the message.
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    ///     Details of the user who sent the message.
    /// </summary>
    public KickUser Sender { get; set; } = new();
}