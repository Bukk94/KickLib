using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class ChatMessageEvent
{
    public string Id { get; set; }
    
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }
    
    public string Content { get; set; }

    // TODO: Convert to enum when all possible types are known
    public string Type { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    public MessageSender Sender { get; set; }
}