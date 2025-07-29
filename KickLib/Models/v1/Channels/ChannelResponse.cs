using KickLib.Models.v1.Categories;
using Newtonsoft.Json;

namespace KickLib.Models.v1.Channels;

/// <summary>
///     Represents a channel information.
/// </summary>
public class ChannelResponse
{
    /// <summary>
    ///     Unique identifier of the broadcaster user associated with the channel.
    /// </summary>
    [JsonProperty(PropertyName = "broadcaster_user_id")]
    public int BroadcasterUserId { get; set; }

    /// <summary>
    ///     URL identifier of the channel.
    /// </summary>
    public string Slug { get; set; } = string.Empty;
    
    /// <summary>
    ///     Description of the channel
    /// </summary>
    [JsonProperty(PropertyName = "channel_description")]
    public string ChannelDescription { get; set; } = string.Empty;
    
    /// <summary>
    ///     URL of the channel's profile picture.
    /// </summary>
    [JsonProperty(PropertyName = "banner_picture")]
    public string BannerPicture { get; set; } = string.Empty;
    
    /// <summary>
    ///     Current stream title.
    /// </summary>
    [JsonProperty(PropertyName = "stream_title")]
    public string StreamTitle { get; set; } = string.Empty;

    /// <summary>
    ///     Current stream details.
    /// </summary>
    public StreamResponse Stream { get; set; } = new();

    /// <summary>
    ///     Current category of the channel.
    /// </summary>
    public CategoryResponse Category { get; set; } = new();
}