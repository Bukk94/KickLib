using KickLib.Client.Models.Events.Chatroom.MessageReply;
using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class ChatMessageEvent
{
    public string Id { get; set; } = string.Empty;
    
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }
    
    public string Content { get; set; } = string.Empty;

    // TODO: Convert to enum when all possible types are known
    // Known values: message, reply
    public string Type { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    public MessageSender Sender { get; set; } = new();

    // Optional metadata
    // Known values: original message data
    public MessageMetadata? Metadata { get; set; }
}