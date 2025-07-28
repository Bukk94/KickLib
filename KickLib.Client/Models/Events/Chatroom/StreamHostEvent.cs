using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class StreamHostEvent
{
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    [JsonProperty(PropertyName = "optional_message")]
    public string? OptionalMessage { get; set; }

    [JsonProperty(PropertyName = "number_viewers")]
    public int NumberViewers { get; set; }

    [JsonProperty(PropertyName = "host_username")]
    public string HostUsername { get; set; } = string.Empty;
}