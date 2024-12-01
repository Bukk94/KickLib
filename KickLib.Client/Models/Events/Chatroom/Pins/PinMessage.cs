using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinMessage
{
    public Guid Id { get; set; }
    
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    public string Content { get; set; }

    public string Type { get; set; }
    
    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    public PinUser Sender { get; set; }
    // Metadata
}