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
    public required string Slug { get; set; }
    
    /// <summary>
    ///     Description of the channel
    /// </summary>
    [JsonProperty(PropertyName = "channel_description")]
    public required string ChannelDescription { get; set; }
    
    /// <summary>
    ///     URL of the channel's profile picture.
    /// </summary>
    [JsonProperty(PropertyName = "banner_picture")]
    public required string BannerPicture { get; set; }
    
    /// <summary>
    ///     Current stream title.
    /// </summary>
    [JsonProperty(PropertyName = "stream_title")]
    public required string StreamTitle { get; set; }

    /// <summary>
    ///     Current stream details.
    /// </summary>
    public required StreamResponse Stream { get; set; }
    
    /// <summary>
    ///     Current category of the channel.
    /// </summary>
    public required CategoryResponse Category { get; set; }
}