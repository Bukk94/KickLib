using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom.MessageReply;

public class MessageMetadata
{
    public required string Id { get; set; }

    [JsonProperty(PropertyName = "original_sender")]
    public required OriginalMessageSender OriginalSender { get; set; }

    [JsonProperty(PropertyName = "original_message")]
    public required OriginalMessage OriginalMessage { get; set; }

    [JsonProperty(PropertyName = "message_ref")] 
    public required string MessageReference { get; set; }
}
