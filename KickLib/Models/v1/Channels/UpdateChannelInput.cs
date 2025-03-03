using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

public class UpdateChannelInput
{
    [JsonProperty(PropertyName = "category_id")]
    public int? CategoryId { get; set; }
    
    [JsonProperty(PropertyName = "stream_title")]
    public string? StreamTitle { get; set; }
}