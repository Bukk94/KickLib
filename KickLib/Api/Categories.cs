using KickLib.Models.v1.Categories;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc />
public class Categories : ApiBase
{
    private const string ApiUrlPart = "categories";

    /// <inheritdoc />
    public Categories(ApiSettings settings, ILogger logger) : base(settings, logger)
    {
    }
    
    /// <summary>
    ///     Gets categories or search for category by name.
    /// </summary>
    public Task<Result<ICollection<CategoryResponse>>> GetCategoriesAsync(
        string searchKeyword, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        List<KeyValuePair<string, string>>? query = null;
        if (!string.IsNullOrWhiteSpace(searchKeyword))
        {
            query = new List<KeyValuePair<string, string>>
            {
                new("q", searchKeyword)
            };
        }

        if (string.IsNullOrWhiteSpace(searchKeyword))
        {
            return Task.FromResult(Result.Fail<ICollection<CategoryResponse>>("searchKeyword is currently required by Kick API!"));
        }
        
        // v1/categories
        return GetAsync<ICollection<CategoryResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Gets specific category by ID.
    /// </summary>
    public Task<Result<CategoryResponse>> GetCategoryAsync(
        int id, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/categories/{id}
        var urlPart = $"{ApiUrlPart}/{id}";
        
        // v1/categories
        return GetAsync<CategoryResponse>(urlPart, ApiVersion.v1, null, accessToken, cancellationToken);
    }
}