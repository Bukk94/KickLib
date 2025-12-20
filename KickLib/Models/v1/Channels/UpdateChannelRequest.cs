namespace KickLib.Models.v1.Channels;

/// <summary>
///     Request to update channel information.
/// </summary>
public class UpdateChannelRequest
{
    /// <summary>
    ///     Category ID to update the channel's category.
    /// </summary>
    public int? CategoryId { get; set; }
    
    /// <summary>
    ///     New stream title.
    /// </summary>
    public string? StreamTitle { get; set; }
    
    /// <summary>
    ///     Custom tags associated with the stream.
    /// </summary>
    public ICollection<string>? CustomTags { get; set; }
}