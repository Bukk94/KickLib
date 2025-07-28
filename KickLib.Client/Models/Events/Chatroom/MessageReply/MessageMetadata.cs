using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom.MessageReply;

public class MessageMetadata
{
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "original_sender")]
    public OriginalMessageSender OriginalSender { get; set; } = new();

    [JsonProperty(PropertyName = "original_message")]
    public OriginalMessage OriginalMessage { get; set; } = new();

    [JsonProperty(PropertyName = "message_ref")] 
    public string MessageReference { get; set; } = string.Empty;
}
