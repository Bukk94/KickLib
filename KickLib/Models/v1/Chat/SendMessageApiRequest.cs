using Newtonsoft.Json;

namespace KickLib.Models.v1.Chat;

internal class SendMessageApiRequest
{
    public SendMessageApiRequest(string content, MessageType type)
    {
        Content = content;
        Type = type;
    }
    
    public string Content { get; }

    public MessageType Type { get; }
    
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int? BroadcasterId { get; set; }
    
    [JsonProperty(PropertyName = "reply_to_message_id")]
    public string? ReplyToMessageId { get; set; }
}