using KickLib.Core;
using KickLib.Interfaces;
using KickLib.Models.Response.v1.Users;

namespace KickLib.Api;

public class Users : BaseApi
{
    private const string ApiUrlPart = "users/";

    public Users(IApiCaller client)
        : base(client)
    {
    }
    
    public Task<UserResponse> GetUserAsync(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentNullException(nameof(username));
        }

        var urlPart = $"{ApiUrlPart}{Uri.EscapeDataString(username)}";
        return GetAsync<UserResponse>(urlPart, ApiVersion.V1);
    }
}