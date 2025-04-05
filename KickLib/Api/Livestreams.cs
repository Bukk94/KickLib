using KickLib.Models.v1.Livestreams;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc />
public class Livestreams : ApiBase
{
    private const string ApiUrlPart = "livestreams";

    /// <inheritdoc />
    public Livestreams(ApiSettings settings, ILogger logger) : base(settings, logger)
    {
    }

    /// <summary>
    ///     Get current Kick Livestreams based on parameters.
    /// </summary>
    /// <paramref name="broadcasterId">Limit results to specific broadcaster (returns single result).</paramref>
    /// <paramref name="categoryId">Limit results to specific category.</paramref>
    /// <paramref name="language">Limit results to specific language.</paramref>
    /// <paramref name="limit">Number of results to return (default: 25, maximum: 100).</paramref>
    /// <paramref name="sort">Result sorting.</paramref>
    public Task<Result<ICollection<LivestreamResponse>>> GetLivestreamsAsync(
        int? broadcasterId = null, 
        int? categoryId = null,
        string? language = null,
        int? limit = null,
        LivestreamSorting? sort = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();
        if (broadcasterId.HasValue)
        {
            query.Add(new("broadcaster_user_id", broadcasterId.ToString()!));
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
}