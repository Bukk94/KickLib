using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class UserBannedEvent
{
    public Guid Id { get; set; }

    public User User { get; set; }
    
    [JsonProperty(PropertyName = "banned_by")]
    public User BannedBy { get; set; }

    public bool Permanent { get; set; }
}