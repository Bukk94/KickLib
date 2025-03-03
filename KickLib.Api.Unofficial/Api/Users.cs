using KickLib.Api.Unofficial.Core;
using KickLib.Api.Unofficial.Interfaces;
using KickLib.Api.Unofficial.Models.Response.v1.Users;
using Microsoft.Extensions.Logging;

namespace KickLib.Api.Unofficial.Api
{
    /// <summary>
    ///     Get information and data about users.
    /// </summary>
    public class Users : BaseApi
    {
        private const string ApiUrlPart = "users/";

        public Users(IApiCaller client, ILogger logger = null)
            : base(client, logger)
        {
        }
    
        /// <summary>
        ///     Gets user information.
        /// </summary>
        /// <param name="username">Username (or slug) to search for.</param>
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
}