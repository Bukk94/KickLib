using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Categories;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="ICategories" />
public class Categories : ApiBase, ICategories
{
    private const string ApiUrlPart = "categories";

    /// <inheritdoc />
    public Categories(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Categories> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }
    
    
    /// <inheritdoc />
    public Task<Result<ICollection<CategoryResponse>>> GetCategoriesAsync(
        string searchKeyword,
        int? page = 1,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchKeyword))
        {
            return Task.FromResult(Result.Fail<ICollection<CategoryResponse>>("searchKeyword is currently required by Kick API!"));
        }
        
        var query = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrWhiteSpace(searchKeyword))
        {
            query.Add(new("q", searchKeyword));
        }

        if (page > 1)
        {
            query.Add(new("page", page.Value.ToString()));
        }
        
        // v1/categories
        return GetAsync<ICollection<CategoryResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
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