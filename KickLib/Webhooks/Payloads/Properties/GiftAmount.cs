using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Contains information about gifted kicks.
/// </summary>
public class GiftAmount
{
    /// <summary>
    ///     Amount of kicks gifted.
    /// </summary>
    public int Amount { get; set; }

    /// <summary>
    ///     Name of the gift.
    /// </summary>
    /// <example>
    ///     Full Send
    /// </example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    ///     Type of the gift
    /// </summary>
    /// <example>
    ///     BASIC
    /// </example>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    ///     Tier of the gift.
    /// </summary>
    /// <example>
    ///     BASIC
    /// </example>
    public string Tier { get; set; } = string.Empty;
    
    /// <summary>
    ///     Gift message.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    ///     Duration in seconds for which the gift is pinned.
    /// </summary>
    [JsonProperty(PropertyName = "pinned_time_seconds")]
    public int PinnedTimeSeconds { get; set; }

    /// <summary>
    ///     Duration for which the gift is pinned.
    /// </summary>
    public TimeSpan PinnedTime => TimeSpan.FromSeconds(PinnedTimeSeconds);
}