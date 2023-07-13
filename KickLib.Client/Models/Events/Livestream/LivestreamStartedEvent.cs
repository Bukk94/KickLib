using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Livestream;

public class LivestreamStartedEvent
{
    public int Id { get; set; }

    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }

    [JsonProperty(PropertyName = "session_title")]
    public string SessionTitle { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }
}