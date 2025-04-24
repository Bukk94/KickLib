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
    Task<Result<ICollection<CategoryResponse>>> GetCategoriesAsync(string searchKeyword, int? page, string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Gets specific category by ID.
    /// </summary>
    Task<Result<CategoryResponse>> GetCategoryAsync(int id, string? accessToken = null, CancellationToken cancellationToken = default);
}
