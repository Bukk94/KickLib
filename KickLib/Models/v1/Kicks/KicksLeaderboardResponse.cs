namespace KickLib.Models.v1.Kicks;

/// <summary>
///     Response when getting KICKs leaderboard.
/// </summary>
public class KicksLeaderboardResponse
{
    /// <summary>
    ///     Lifetime leaderboard entries.
    /// </summary>
    public List<KicksLeaderboardEntry> Lifetime { get; set; } = [];

    /// <summary>
    ///     Monthly leaderboard entries.
    /// </summary>
    public List<KicksLeaderboardEntry> Month { get; set; } = [];

    /// <summary>
    ///     Weekly leaderboard entries.
    /// </summary>
    public List<KicksLeaderboardEntry> Week { get; set; } = [];
}