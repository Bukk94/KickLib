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
    /// <param name="broadcasterId">Limit results to specific broadcaster (returns single result).</param>
    /// <param name="categoryId">Limit results to specific category.</param>
    /// <param name="language">Limit results to specific language.</param>
    /// <param name="limit">Number of results to return (default: 25, maximum: 100).</param>
    /// <param name="sort">Result sorting.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
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
    /// <param name="broadcasterIds">Limit results to specific broadcasters.</param>
    /// <param name="categoryId">Limit results to specific category.</param>
    /// <param name="language">Limit results to specific language.</param>
    /// <param name="limit">Number of results to return (default: 25, maximum: 100).</param>
    /// <param name="sort">Result sorting.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        ICollection<int> broadcasterIds,
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get livestream information for currently authorised user.
    /// </summary>
    /// <returns>Returns <c>null</c> if user is not livestreaming.</returns>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<LivestreamResponse?>> GetLivestreamAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default);
}
