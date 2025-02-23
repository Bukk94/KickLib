using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinMessage
{
    public Guid Id { get; set; }
    
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    public required string Content { get; set; }

    /// <summary>
    ///     Possible known values:
    ///     message
    /// </summary>
    public required string Type { get; set; }
    
    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    public required PinUser Sender { get; set; }
    
    // Metadata - nullable
}