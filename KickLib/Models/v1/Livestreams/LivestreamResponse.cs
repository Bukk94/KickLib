using KickLib.Models.v1.Categories;
using Newtonsoft.Json;

namespace KickLib.Models.v1.Livestreams;

/// <summary>
///     Response when getting livestreams list.
/// </summary>
public class LivestreamResponse
{
    /// <summary>
    ///     Unique identifier of the broadcaster user associated with the channel.
    /// </summary>
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int BroadcasterUserId { get; set; }
    
    /// <summary>
    ///     Livestream Category.
    /// </summary>
    public required CategoryResponse Category { get; set; }
    
    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }
    
    [JsonProperty(PropertyName = "has_mature_content")]
    public bool HasMatureContent { get; set; }

    public string? Language { get; set; }
    
    public required string Slug { get; set; }
    
    [JsonProperty(PropertyName = "started_at")]
    public DateTimeOffset? StartedAt { get; set; }
    
    [JsonProperty(PropertyName = "stream_title")]
    public string? StreamTitle { get; set; }
    
    public string? Thumbnail { get; set; }
    
    [JsonProperty(PropertyName = "viewer_count")]
    public int ViewerCount { get; set; }
}