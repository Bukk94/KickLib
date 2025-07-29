using Newtonsoft.Json;

namespace KickLib.Auth
{
    /// <summary>
    ///     Response from the Kick API when requesting a new user access token.
    /// </summary>
    public class KickTokenResponse
    {
        /// <summary>
        ///     Access (Bearer) token.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        ///     Refresh token (used for requesting more access tokens).
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;
        
        /// <summary>
        ///     Type of the token.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;

        /// <summary>
        ///     Allowed scopes.
        /// </summary>
        public string Scope { get; set; } = string.Empty;
        
        /// <summary>
        ///     Expiration time of the token in seconds.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}