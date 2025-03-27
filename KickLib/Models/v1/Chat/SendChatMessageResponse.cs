using Newtonsoft.Json;

namespace KickLib.Models.v1.Chat;

/// <summary>
///     Response when sending a chat message.
/// </summary>
public class SendChatMessageResponse
{
    /// <summary>
    ///     Whether the message was sent successfully.
    /// </summary>
    [JsonProperty(PropertyName = "is_sent")]
    public bool IsSent { get; set; }

    /// <summary>
    ///     Unique identifier of the message.
    /// </summary>
    [JsonProperty(PropertyName = "message_id")]
    public string? MessageId { get; set; }
}