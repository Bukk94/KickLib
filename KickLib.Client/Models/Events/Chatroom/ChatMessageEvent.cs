using KickLib.Client.Models.Events.Chatroom.MessageReply;
using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class ChatMessageEvent
{
    public required string Id { get; set; }
    
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }
    
    public required string Content { get; set; }

    // TODO: Convert to enum when all possible types are known
    // Known values: message, reply
    public required string Type { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    public required MessageSender Sender { get; set; }

    // Optional metadata
    // Known values: original message data
    public MessageMetadata? Metadata { get; set; }
}