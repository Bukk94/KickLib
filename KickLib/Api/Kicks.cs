using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Kicks;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IKicks" />
public class Kicks : ApiBase, IKicks
{
    private const string ApiUrlPart = "kicks";

    /// <inheritdoc />
    public Kicks(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Kicks> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }
    
    /// <inheritdoc />
    public async Task<Result<KicksLeaderboardResponse>> GetKicksLeaderboardAsync(
        int? top = null,
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        var query = new List<KeyValuePair<string, string>>();

        if (top.HasValue)
        {
            if (top < 1 || top > 100)
            {
                return Result.Fail<KicksLeaderboardResponse>("Top must be value between 1 and 100!");
                
            }
            query.Add(new("top", top.ToString()!));
        }
        
        // v1/kicks/leaderboard
        var urlPart = $"{ApiUrlPart}/leaderboard";
        var result = await GetAsync<KicksLeaderboardResponse>(urlPart, ApiVersion.v1, query, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.KicksRead}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<KicksLeaderboardResponse>(result.Errors);
        }

        return Result.Ok(result.Value).WithSuccesses(result.Successes);
    }
}