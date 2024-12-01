using Newtonsoft.Json;

namespace KickLib.Models.Response.v1.Videos;

public class VideoVerifiedResponse
{
    public int Id { get; set; }
    
    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }
    
    [JsonProperty(PropertyName = "created_at")]
    public DateTime? CreatedAt { get; set; }
    
    [JsonProperty(PropertyName = "updated_at")]
    public DateTime? UpdatedAt { get; set; }
}