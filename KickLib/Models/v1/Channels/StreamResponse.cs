using KickLib.Models.v1.Categories;
using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

public class StreamResponse
{
    /// <summary>
    ///     Requires streamkey:read scope
    /// </summary>
    public string? Url { get; set; }
    
    /// <summary>
    ///     Requires streamkey:read scope
    /// </summary>
    public string? Key { get; set; }
    
    public required string Language { get; set; }
    
    [JsonProperty(PropertyName = "is_live")]
    public bool IsLive { get; set; }
    
    [JsonProperty(PropertyName = "is_mature")]
    public bool IsMature { get; set; }
    
    [JsonProperty(PropertyName = "start_time")]
    public required DateTimeOffset StartTime { get; set; }
    
    [JsonProperty(PropertyName = "viewer_count")]
    public int ViewerCount { get; set; }
}