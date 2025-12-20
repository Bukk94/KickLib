using Newtonsoft.Json;

namespace KickLib.Models.v1.ChannelRewards;

/// <summary>
///     Represents a channel reward.
/// </summary>
public class ChannelReward
{
    /// <summary>
    ///     Reward unique identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    ///     Background color of the reward in hexadecimal format.
    /// </summary>
    /// <example>
    ///     #00e701
    /// </example>>
    [JsonProperty(PropertyName = "background_color")]
    public string BackgroundColor { get; set; } = string.Empty;
    
    /// <summary>
    ///     Cost of the reward in channel points.
    /// </summary>
    public int Cost { get; set; }
    
    /// <summary>
    ///     Description of the reward.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    ///     Indicates whether the reward is currently enabled.
    /// </summary>
    [JsonProperty(PropertyName = "is_enabled")]
    public bool IsEnabled { get; set; }
    
    /// <summary>
    ///     Indicates whether the reward is currently paused.
    /// </summary>
    [JsonProperty(PropertyName = "is_paused")]
    public bool IsPaused { get; set; }
    
    /// <summary>
    ///     Indicates whether user input is required when redeeming the reward.
    /// </summary>
    [JsonProperty(PropertyName = "is_user_input_required")]
    public bool IsUserInputRequired { get; set; }
    
    /// <summary>
    ///     Indicates whether redemptions of this reward should skip the request queue.
    /// </summary>
    [JsonProperty(PropertyName = "should_redemptions_skip_request_queue")]
    public bool ShouldRedemptionsSkipRequestQueue { get; set; }
    
    /// <summary>
    ///     Title of the reward.
    /// </summary>
    public string Title { get; set; } = string.Empty;
}