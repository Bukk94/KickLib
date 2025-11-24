using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

/// <summary>
///     Stream details. 
/// </summary>
public class StreamResponse
{
    /// <summary>
    ///     Streaming URL for Streaming software. Requires streamkey:read scope.
    /// </summary>
    public string? Url { get; set; }
    
    /// <summary>
    ///     Streaming Key for Streaming software. Requires streamkey:read scope.
    /// </summary>
    public string? Key { get; set; }
    
    /// <summary>
    ///     Current language of the stream.
    /// </summary>
    public string Language { get; set; } = string.Empty;
    
    /// <summary>
    ///     Indicates if the stream is live.
    /// </summary>
    [JsonProperty(PropertyName = "is_live")]
    public bool IsLive { get; set; }
    
    /// <summary>
    ///     Indicates if the stream is for mature audience.
    /// </summary>
    [JsonProperty(PropertyName = "is_mature")]
    public bool IsMature { get; set; }
    
    /// <summary>
    ///     Start time of the stream.
    /// </summary>
    [JsonProperty(PropertyName = "start_time")]
    public DateTimeOffset? StartTime { get; set; }
    
    /// <summary>
    ///     Number of viewers currently watching the stream.
    /// </summary>
    [JsonProperty(PropertyName = "viewer_count")]
    public int ViewerCount { get; set; }

    /// <summary>
    ///     URL to a stream thumbnail.
    /// </summary>
    public string Thumbnail { get; set; } = string.Empty;

    /// <summary>
    ///     Custom tags associated with the stream.
    /// </summary>
    [JsonProperty(PropertyName = "custom_tags")]
    public ICollection<string> CustomTags { get; set; } = [];
}