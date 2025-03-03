using KickLib.Models.v1.Categories;
using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

public class ChannelResponse
{
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int BroadcasterUserId { get; set; }

    public required string Slug { get; set; }
    
    [JsonProperty(PropertyName = "channel_description")]
    public required string ChannelDescription { get; set; }
    
    [JsonProperty(PropertyName = "banner_picture")]
    public required string BannerPicture { get; set; }
    
    [JsonProperty(PropertyName = "stream_title")]
    public required string StreamTitle { get; set; }

    public required StreamResponse Stream { get; set; }
    
    public required CategoryResponse Category { get; set; }
}