namespace KickLib.Models.v1.ChannelRewards.Redemptions;

/// <summary>
///     Represents a collection of reward redemptions along with their associated reward details.
/// </summary>
public class ChannelRewardRedemption
{
    /// <summary>
    ///     A collection of reward redemptions.
    /// </summary>
    public ICollection<RewardRedemption> Redemptions { get; set; } = [];
    
    /// <summary>
    ///     The details of the reward associated with the redemptions.
    /// </summary>
    public RedemptionRewardDetails Reward { get; set; } = new();
}