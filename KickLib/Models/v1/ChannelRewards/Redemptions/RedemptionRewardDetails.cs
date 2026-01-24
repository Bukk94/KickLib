using Newtonsoft.Json;

namespace KickLib.Models.v1.ChannelRewards.Redemptions;

/// <summary>
///     Details about a channel reward associated with redemptions.
/// </summary>
public class RedemptionRewardDetails
{
    /// <summary>
    ///     Reward unique identifier.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    ///     Title of the reward.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    ///     Cost of the reward in channel points.
    /// </summary>
    public int Cost { get; set; }
    
    /// <summary>
    ///     Description of the reward.
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    ///     Indicates whether the current user can manage this reward.
    /// </summary>
    [JsonProperty(PropertyName = "can_manage")]
    public bool? CanManage { get; set; }
    
    /// <summary>
    ///     Indicates whether the reward has been deleted.
    /// </summary>
    [JsonProperty(PropertyName = "is_deleted")]
    public bool? IsDeleted { get; set; }
}