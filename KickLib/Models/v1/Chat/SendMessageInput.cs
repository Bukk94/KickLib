using Newtonsoft.Json;

namespace KickLib.Models.v1.Chat;

internal class SendMessageInput
{
    public SendMessageInput(string content, MessageType type)
    {
        Content = content;
        Type = type;
    }
    
    public string Content { get; }

    public MessageType Type { get; }
    
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int? BroadcasterId { get; set; }
}