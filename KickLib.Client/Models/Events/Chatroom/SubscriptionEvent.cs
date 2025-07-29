using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class SubscriptionEvent
{
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    public string Username { get; set; } = string.Empty;

    public int Months { get; set; }
}