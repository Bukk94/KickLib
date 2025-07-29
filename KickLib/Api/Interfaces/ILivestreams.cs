using KickLib.Models.v1.Livestreams;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Interact with livestreams on Kick.com.
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
    Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        int? broadcasterId = null, 
        int? categoryId = null, 
        string? language = null, 
        int? limit = null, 
        LivestreamSorting? sort = null, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get current Kick Livestreams based on parameters.
    /// </summary>
    /// <paramref name="broadcasterIds">Limit results to specific broadcasters.</paramref>
    /// <paramref name="categoryId">Limit results to specific category.</paramref>
    /// <paramref name="language">Limit results to specific language.</paramref>
    /// <paramref name="limit">Number of results to return (default: 25, maximum: 100).</paramref>
    /// <paramref name="sort">Result sorting.</paramref>
    Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        ICollection<int> broadcasterIds,
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
