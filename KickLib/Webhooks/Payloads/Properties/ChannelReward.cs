namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a channel reward.
/// </summary>
public class ChannelReward
{
    /// <summary>
    ///     The unique identifier of the reward.
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    ///     The title of the reward.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    ///     The cost of the reward in channel points.
    /// </summary>
    public int Cost { get; set; }

    /// <summary>
    ///     The description of the reward.
    /// </summary>
    public string Description { get; set; } = string.Empty;
}