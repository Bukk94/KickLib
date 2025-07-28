using Newtonsoft.Json;

namespace KickLib.Auth
{
    /// <summary>
    ///     Response from the Kick API when requesting a new app access token.
    /// </summary>
    public class KickAppTokenResponse
    {
        /// <summary>
        ///     Access (Bearer) token.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        ///     Type of the token.
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; } = string.Empty;

        /// <summary>
        ///     Expiration time of the token in seconds.
        /// </summary>
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}