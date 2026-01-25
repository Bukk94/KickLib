using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1;
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
    [Obsolete("Keyword search and pagination via page numbers is deprecated and will be removed in future versions. Kick stopped supporting it in their API. Use cursor instead.")]
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
    public Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesAsync(
        int? limit = null,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query.Add(new("cursor", cursor));
        }
        
        if (limit.HasValue)
        {
            query.Add(new("limit", limit.Value.ToString()));
        }

        return GetCategoriesInternalAsync(query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesAsync(
        GetCategoriesRequest request,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (request is null)
        {
            throw new ArgumentException("Request cannot be null.", nameof(request));
        }

        var query = new List<KeyValuePair<string, string>>();
        if (request.Limit.HasValue)
        {
            query.Add(new("limit", request.Limit.Value.ToString()));
        }
        
        if (!string.IsNullOrWhiteSpace(request.Cursor))
        {
            query.Add(new("cursor", request.Cursor));
        }
        
        if (request.Names != null && request.Names.Any())
        {
            query.AddRange(request.Names.Distinct().Select(name => new KeyValuePair<string, string>("name", name)));
        }
        
        if (request.Tags != null && request.Tags.Any())
        {
            query.AddRange(request.Tags.Distinct().Select(tag => new KeyValuePair<string, string>("tag", tag)));
        }
        
        if (request.Ids != null && request.Ids.Any())
        {
            query.AddRange(request.Ids.Distinct().Select(id => new KeyValuePair<string, string>("id", id.ToString())));
        }

        return GetCategoriesInternalAsync(query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesByNameAsync(
        string name,
        int? limit = null,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Category name must be provided.", nameof(name));
        }

        var query = new List<KeyValuePair<string, string>>
        {
            new("name", name)
        };
        
        if (limit.HasValue)
        {
            query.Add(new("limit", limit.Value.ToString()));
        }
        
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query.Add(new("cursor", cursor));
        }

        return GetCategoriesInternalAsync(query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesByTagAsync(
        string tag,
        int? limit = null,
        string? cursor = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(tag))
        {
            throw new ArgumentException("Category name must be provided.", nameof(tag));
        }

        var query = new List<KeyValuePair<string, string>>
        {
            new("tag", tag)
        };
        
        if (limit.HasValue)
        {
            query.Add(new("limit", limit.Value.ToString()));
        }
        
        if (!string.IsNullOrWhiteSpace(cursor))
        {
            query.Add(new("cursor", cursor));
        }

        return GetCategoriesInternalAsync(query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<CategoryResponse>> GetCategoryAsync(
        int id, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v2/categories
        var query = new List<KeyValuePair<string, string>>
        {
            new("id", id.ToString())
        };
        
        var result = await GetAsync<ICollection<CategoryResponse>>(ApiUrlPart, ApiVersion.v2, query, accessToken, cancellationToken)
            .ConfigureAwait(false);

        if (result.IsFailed)
        {
            return Result.Fail<CategoryResponse>(result.Errors);
        }
        
        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    private async Task<Result<PaginatedResponse<ICollection<CategoryResponse>>>> GetCategoriesInternalAsync(
        List<KeyValuePair<string, string>> query,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var result = await GetAsync<ICollection<CategoryResponse>>(ApiUrlPart, ApiVersion.v2, query, accessToken, cancellationToken).ConfigureAwait(false);
 
        if (result.IsFailed)
        {
            return Result.Fail<PaginatedResponse<ICollection<CategoryResponse>>>(result.Errors);
        }

        var pagination = result.Successes.OfType<ResponseMetadata>().FirstOrDefault()?.GetPagination();

        return Result.Ok(new PaginatedResponse<ICollection<CategoryResponse>>
        {
            Data = result.Value,
            NextCursor = pagination?.NextCursor
        });
    }
}