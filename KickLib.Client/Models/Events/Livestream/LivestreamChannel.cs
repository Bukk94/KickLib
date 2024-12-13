using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Livestream;

public class LivestreamChannel
{
    public int Id { get; set; }

    [JsonProperty(PropertyName = "is_banned")]
    public bool IsBanned { get; set; }
}