using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class UserUnbannedEvent
{
    public Guid Id { get; set; }

    public required User User { get; set; }
    
    [JsonProperty(PropertyName = "unbanned_by")]
    public required User BannedBy { get; set; }
}