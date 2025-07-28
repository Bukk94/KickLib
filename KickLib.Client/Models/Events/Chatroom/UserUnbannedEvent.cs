using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class UserUnbannedEvent
{
    public Guid Id { get; set; }

    public User User { get; set; } = new();

    [JsonProperty(PropertyName = "unbanned_by")]
    public User BannedBy { get; set; } = new();
}