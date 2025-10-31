using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Users;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IUsers" />
public class Users : ApiBase, IUsers
{
    private const string ApiUrlPart = "users";

    /// <inheritdoc />
    public Users(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Users> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
    }
    
    /// <inheritdoc />
    public async Task<Result<UserResponse>> GetUserAsync(
        int userId, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        List<KeyValuePair<string, string>> query =
        [
            new("id", userId.ToString())
        ];
        
        // v1/users
        var result = await GetAsync<ICollection<UserResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.IsFailed)
        {
            return Result.Fail<UserResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
    
    /// <inheritdoc />
    public Task<Result<ICollection<UserResponse>>> GetUsersAsync(
        ICollection<int>? userIds, 
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        List<KeyValuePair<string, string>>? query = null;
        if (userIds?.Count > 0)
        {
            query = [];
            foreach (var id in userIds.Distinct())
            {
                query.Add(new("id", id.ToString()));
            }
        }
        
        // v1/users
        return GetAsync<ICollection<UserResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken, cancellationToken);
    }
    
    /// <inheritdoc />
    public async Task<Result<UserResponse>> GetMeAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/users
        var result = await GetAsync<ICollection<UserResponse>>(ApiUrlPart, ApiVersion.v1, null, accessToken, cancellationToken)
            .ConfigureAwait(false);
        
        if (result.HasError(x => x.Message == "Response code: 403"))
        {
            result.WithError($"Missing scope: {KickScopes.UserRead}");
        }
        
        if (result.IsFailed)
        {
            return Result.Fail<UserResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
}