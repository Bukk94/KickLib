namespace KickLib.Models.v1.ChannelRewards.Redemptions;

/// <summary>
///     Represents a failed redemption status change attempt.
/// </summary>
public class FailedRedemptions
{
    /// <summary>
    ///     The unique identifier of the failed redemption.
    /// </summary>
    public string Id { get; set; } = string.Empty;
    
    /// <summary>
    ///     The reason for the failure.
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}