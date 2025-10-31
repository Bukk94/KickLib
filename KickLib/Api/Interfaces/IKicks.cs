using KickLib.Models.v1.Kicks;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Endpoints related to KICKs.
/// </summary>
public interface IKicks
{
    /// <summary>
    ///     Gets the KICKs leaderboard for the authenticated broadcaster.
    /// </summary>
    /// <remarks>
    ///     Required scope: kicks:read
    /// </remarks>
    /// <param name="top">The number of entries from the top of the leaderboard to return. For example, 10 will fetch the top 10 entries. Minimum 1, maximum: 100.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<KicksLeaderboardResponse>> GetKicksLeaderboardAsync(
        int? top = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}