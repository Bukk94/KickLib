using Newtonsoft.Json;

namespace KickLib.Auth
{
    /// <summary>
    ///     Response from the Kick API when requesting a new token.
    /// </summary>
    public class KickTokenResponse
    {
        /// <summary>
        ///     Access (Bearer) token.
        /// </summary>
        [JsonProperty("access_token")]
        public required string AccessToken { get; set; }

        /// <summary>
        ///     Refresh token (used for requesting more access tokens).
        /// </summary>
        [JsonProperty("refresh_token")]
        public required string RefreshToken { get; set; }
        
        /// <summary>
        ///     Type of the token.
        /// </summary>
        [JsonProperty("token_type")]
        public required string TokenType { get; set; }

        /// <summary>
        ///     Expiration time of the token in seconds.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        /// <summary>
        ///     Allowed scopes.
        /// </summary>
        public required string Scope { get; set; }
    }
}