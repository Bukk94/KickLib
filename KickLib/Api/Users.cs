using KickLib.Models.v1.Users;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

public class Users : ApiBase
{
    private const string ApiUrlPart = "users";

    public Users(ApiSettings settings, ILogger logger) 
        : base(settings, logger)
    {
    }
    
    /// <summary>
    ///     Retrieve user information based on provided user IDs.
    ///     If no user IDs are specified, the information for the currently authorised user will be returned by default.
    /// </summary>
    public Task<Result<ICollection<UserResponse>>> GetUsersAsync(ICollection<int>? userIds, string? accessToken = null)
    {
        List<KeyValuePair<string, string>>? query = null;
        if (userIds?.Any() == true)
        {
            query = new List<KeyValuePair<string, string>>();
            foreach (var id in userIds.Distinct())
            {
                query.Add(new("id", id.ToString()));
            }
        }
        
        // v1/users
        return GetAsync<ICollection<UserResponse>>(ApiUrlPart, ApiVersion.v1, query, accessToken);
    }
    
    /// <summary>
    ///     Retrieve information for the currently authorised user.
    /// </summary>
    public async Task<Result<UserResponse>> GetMeAsync(string? accessToken = null)
    {
        // v1/users
        var result = await GetAsync<ICollection<UserResponse>>(ApiUrlPart, ApiVersion.v1, null, accessToken).ConfigureAwait(false);
        if (result.IsFailed)
        {
            return Result.Fail<UserResponse>(result.Errors);
        }

        return Result.Ok(result.Value.First()).WithSuccesses(result.Successes);
    }
}