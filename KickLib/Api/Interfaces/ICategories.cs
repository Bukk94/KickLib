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
    Task<Result<ICollection<CategoryResponse>>> GetCategoriesAsync(
        string searchKeyword, 
        int? page, 
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
