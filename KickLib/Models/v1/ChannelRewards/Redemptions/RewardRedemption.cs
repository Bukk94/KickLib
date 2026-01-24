using Newtonsoft.Json;

namespace KickLib.Models.v1.ChannelRewards.Redemptions;

/// <summary>
///     Represents a reward redemption.
/// </summary>
public class RewardRedemption
{
    /// <summary>
    ///     The unique identifier of the reward redemption.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    ///     The timestamp when the reward was redeemed.
    /// </summary>
    [JsonProperty(PropertyName = "redeemed_at")]
    public DateTimeOffset RedeemedAt { get; set; }
    
    /// <summary>
    ///     The user who redeemed the reward.
    /// </summary>
    public Redeemer Redeemer { get; set; } = new();
    
    /// <summary>
    ///     Details about the reward associated with this redemption.
    /// </summary>
    public RedemptionStatus Status { get; set; }
    
    /// <summary>
    ///     Additional user input provided during the redemption.
    /// </summary>
    [JsonProperty(PropertyName = "user_input")]
    public string UserInput { get; set; } = string.Empty;
}