using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     A Livestream metadata.
/// </summary>
public class LivestreamMetadata
{
    /// <summary>
    ///     Livestream title.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    ///     Livestream language.
    /// </summary>
    public string Language { get; set; } = string.Empty;
    
    /// <summary>
    ///     Livestream category.
    /// </summary>
    public Category Category { get; set; } = new();
    
    /// <summary>
    ///     Does livestream have mature content?
    /// </summary>
    [JsonProperty(PropertyName = "has_mature_content")]
    public bool HasMatureContent { get; set; }
}