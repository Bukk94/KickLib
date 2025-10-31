using Newtonsoft.Json;

namespace KickLib.Models.v1.Kicks;

/// <summary>
///     Entry in the KICKs leaderboard.
/// </summary>
public class KicksLeaderboardEntry
{
    /// <summary>
    ///     Amount of KICKs gifted.
    /// </summary>
    [JsonProperty(PropertyName = "gifted_amount")]
    public int GiftedAmount { get; set; }

    /// <summary>
    ///     Rank in the leaderboard.
    /// </summary>
    public int Rank { get; set; }

    /// <summary>
    ///     User identifier.
    /// </summary>
    [JsonProperty(PropertyName = "user_id")]
    public int UserId { get; set; }

    /// <summary>
    ///     Username of the user.
    /// </summary>
    public string Username { get; set; } = string.Empty;
}