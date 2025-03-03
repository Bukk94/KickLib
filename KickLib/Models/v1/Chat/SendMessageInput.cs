using Newtonsoft.Json;

namespace KickLib.Models.v1.Chat;

internal class SendMessageInput
{
    public required string Content { get; set; }

    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int? BroadcasterId { get; set; }

    public MessageType Type { get; set; }
}