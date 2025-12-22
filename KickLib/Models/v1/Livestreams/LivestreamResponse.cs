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
    public CategoryResponse Category { get; set; } = new();
    
    /// <summary>
    ///     Channel identifier where the livestream is hosted.
    /// </summary>
    [JsonProperty(PropertyName = "channel_id")]
    public int ChannelId { get; set; }
    
    /// <summary>
    ///     Custom tags associated with the stream.
    /// </summary>
    [JsonProperty(PropertyName = "custom_tags")]
    public ICollection<string> CustomTags { get; set; } = [];
    
    /// <summary>
    ///     Has livestream mature content?
    /// </summary>
    [JsonProperty(PropertyName = "has_mature_content")]
    public bool HasMatureContent { get; set; }

    /// <summary>
    ///     Language of the livestream.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    ///     Broadcaster's profile picture.
    /// </summary>
    [JsonProperty(PropertyName = "profile_picture")]
    public string ProfilePicture { get; set; } = string.Empty;
    
    /// <summary>
    ///     Slug identifier of the livestream host.
    /// </summary>
    public string Slug { get; set; } = string.Empty;
    
    /// <summary>
    ///     When the livestream started.
    /// </summary>
    [JsonProperty(PropertyName = "started_at")]
    public DateTimeOffset? StartedAt { get; set; }
    
    /// <summary>
    ///     Livestream title.
    /// </summary>
    [JsonProperty(PropertyName = "stream_title")]
    public string? StreamTitle { get; set; }
    
    /// <summary>
    ///     Livestream thumbnail URL.
    /// </summary>
    public string? Thumbnail { get; set; }
    
    /// <summary>
    ///     Current livestream viewers count.
    /// </summary>
    [JsonProperty(PropertyName = "viewer_count")]
    public int ViewerCount { get; set; }
}