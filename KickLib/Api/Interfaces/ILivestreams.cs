using KickLib.Models.v1.Livestreams;

namespace KickLib.Api.Interfaces;

/// <summary>
/// Interact with livestreams on kick.com
/// </summary>
public interface ILivestreams
{
    /// <summary>
    ///     Get current Kick Livestreams based on parameters.
    /// </summary>
    /// <paramref name="broadcasterId">Limit results to specific broadcaster (returns single result).</paramref>
    /// <paramref name="categoryId">Limit results to specific category.</paramref>
    /// <paramref name="language">Limit results to specific language.</paramref>
    /// <paramref name="limit">Number of results to return (default: 25, maximum: 100).</paramref>
    /// <paramref name="sort">Result sorting.</paramref>
    Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(int? broadcasterId, int? categoryId, string? language, int? limit, LivestreamSorting? sort, string? accessToken, CancellationToken cancellationToken);


}
