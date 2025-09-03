using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Livestreams;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="ILivestreams" />
public class Livestreams : ApiBase, ILivestreams
{
    private const string ApiUrlPart = "livestreams";

    /// <inheritdoc />
    public Livestreams(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Livestreams> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }

    /// <inheritdoc />
    public Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        int? broadcasterId = null, 
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var broadcasterIds = broadcasterId is null 
            ? [] 
            : new List<int> { broadcasterId.Value };
        
        return GetLivestreamsAsync(broadcasterIds, categoryId, language, limit, sort, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        ICollection<int> broadcasterIds, 
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        if (broadcasterIds?.Any() == true)
        {
            foreach (var id in broadcasterIds.Distinct())
            {
                query.Add(new("broadcaster_user_id", id.ToString()));
            }
        }
        
        if (categoryId.HasValue)
        {
            query.Add(new("category_id", categoryId.ToString()!));
        }
        
        if (!string.IsNullOrWhiteSpace(language))
        {
            query.Add(new("language", language));
        }
        
        if (limit.HasValue)
        {
            if (limit < 1 || limit > 100)
            {
                return Task.FromResult(Result.Fail<ICollection<LivestreamResponse>>("Limit must be value between 1 and 100!"));
            }
            
            query.Add(new("limit", limit.ToString()!));
        }
        
        if (sort.HasValue)
        {
            var sortValue = sort switch 
            {
                LivestreamSorting.ByViewerCount => "viewer_count",
                LivestreamSorting.ByStartTime => "started_at",
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };
            
            query.Add(new("sort", sortValue));
        }
        
        // v1/livestreams
        return GetAsync<ICollection<LivestreamResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<LivestreamResponse?>> GetLivestreamAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/livestreams/stats
        var urlPart = $"{ApiUrlPart}/stats";
        
        var result = await GetAsync<ICollection<LivestreamResponse>>(urlPart, ApiVersion.v1, null, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.IsFailed)
        {
            return Result.Fail<LivestreamResponse?>(result.Errors);
        }

        return Result.Ok(
            result.Value.Any() ? result.Value.First() : null)
            .WithSuccesses(result.Successes);
    }
}