using Newtonsoft.Json;

namespace KickLib.Models.v1.ChannelRewards.Redemptions;

/// <summary>
///     Represents the user who redeemed a reward.
/// </summary>
public class Redeemer
{
    /// <summary>
    ///     Unique identifier of the user who redeemed the reward.
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }
}