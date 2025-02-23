using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class SubscriptionEvent
{
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    public required string Username { get; set; }

    public int Months { get; set; }
}