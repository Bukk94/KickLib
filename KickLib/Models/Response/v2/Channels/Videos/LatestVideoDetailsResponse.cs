using Newtonsoft.Json;

namespace KickLib.Models.Response.v2.Channels.Videos;

public class LatestVideoDetailsResponse
{
    public long Id { get; set; }

    public Guid Uuid { get; set; }

    [JsonProperty(PropertyName = "live_stream_id")]
    public long LivestreamId { get; set; }
}