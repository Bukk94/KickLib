using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a reward redemption update event.
/// </summary>
public class ChannelRewardRedemptionUpdatedEvent : WebhookEventBase
{
    /// <summary>
    ///     The unique identifier of the reward redemption.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     User's input provided when redeeming the reward.
    /// </summary>
    [JsonProperty(PropertyName = "user_input")]
    public string UserInput { get; set; } = string.Empty;

    /// <summary>
    ///      Redemption status.
    /// </summary>
    public RedemptionStatus Status { get; set; }
    
    /// <summary>
    ///     Date and time when the reward was redeemed.
    /// </summary>
    [JsonProperty(PropertyName = "redeemed_at")]
    public DateTimeOffset RedeemedAt { get; set; }

    /// <summary>
    ///      The reward associated with the redemption.
    /// </summary>
    public ChannelReward Reward { get; set; } = new();
    
    /// <summary>
    ///     Details about the user who redeemed the reward.
    /// </summary>
    public KickUser Redeemer { get; set; } = new();
}
