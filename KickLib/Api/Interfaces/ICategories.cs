using KickLib.Models.v1;
using KickLib.Models.v1.Categories;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Category operations.
/// </summary>
public interface ICategories
{
    /// <summary>
    ///     Gets categories or search for category by name.
    ///     Returns up to 100 results at a time; use the <paramref name="page"/> parameter to get more results.
    /// </summary>
    /// <param name="searchKeyword">Keyword to search categories by name.</param>
    /// <param name="page">Page number for paginated results (each page contains up to 100 results).</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    [Obsolete("Keyword search and pagination via page numbers is deprecated and will be removed in future versions. Kick stopped supporting it in their API. Use cursor instead.")]
    Task<Result<ICollection<CategoryResponse>>> GetCategoriesAsync(
        string searchKeyword, 
        int? page, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets categories.
    /// </summary>
    /// <param name="limit">Limit how many results to return.</param>
    /// <param name="cursor">Cursor value to get next page of results.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesAsync(
        int? limit = null,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets categories.
    /// </summary>
    /// <param name="request">Complex request object.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesAsync(
        GetCategoriesRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get categories by name.
    /// </summary>
    /// <param name="name">Name of the category to filter by.</param>
    /// <param name="limit">Limit how many results to return.</param>
    /// <param name="cursor">Cursor value to get next page of results.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesByNameAsync(
        string name,
        int? limit = null,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get categories by tag.
    /// </summary>
    /// <param name="tag">Name of the tag to filter categories by.</param>
    /// <param name="limit">Limit how many results to return.</param>
    /// <param name="cursor">Cursor value to get next page of results.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesByTagAsync(
        string tag,
        int? limit = null,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    ///     Gets specific category by ID.
    /// </summary>
    /// <param name="id">Category identifier.</param>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<CategoryResponse>> GetCategoryAsync(
        int id, 
        string? accessToken = null, 
        CancellationToken cancellationToken = default);
}
