using Newtonsoft.Json;

namespace KickLib.Models.v1.Chat;

public class SendChatMessageResponse
{
    [JsonProperty(PropertyName = "is_sent")]
    public bool IsSent { get; set; }

    [JsonProperty(PropertyName = "message_id")]
    public string? MessageId { get; set; }
}