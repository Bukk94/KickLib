namespace KickLib.Auth;

/// <summary>
///     KickLib service for generating OAuth 2.1 authorization flow (URLs, exchanging tokens, revocation).
/// </summary>
public interface IKickOAuthGenerator
{
    /// <summary>
    ///     [User Access Token] Generate the OAuth authorization URL.
    ///     When state is provided, it will be used. Otherwise Base64 encoded verifier will be used (unsafe).
    /// </summary>
    /// <param name="redirectUri">Callback redirect URL (must be registered in the app).</param>
    /// <param name="clientId">App Client ID.</param>
    /// <param name="scopes">A list of scopes to grant.</param>
    /// <param name="verifier">Used verifier code value before hashing.</param>
    /// <param name="state">Validation state (if null, base64 encoded verifier code will be used).</param>
    /// <returns>Returns authorization Uri.</returns>
    Uri GetAuthorizationUri(string redirectUri, string clientId, ICollection<string> scopes, out string verifier, string? state = null);

    /// <summary>
    ///     [User Access Token] Exchange the code from callback for an access token.
    /// </summary>
    /// <param name="code">Code received from callback.</param>
    /// <param name="clientId">App client ID.</param>
    /// <param name="clientSecret">App secret.</param>
    /// <param name="redirectUrl">Redirect URL used during initial request.</param>
    /// <param name="state">Received state from the callback.</param>
    /// <param name="verifier">Verifier code used for initial authentication call (if null, it will try to decode state value).</param>
    /// <returns>Returns Kick token response (access/refresh tokens).</returns>
    Task<Result<KickTokenResponse>> ExchangeCodeForTokenAsync(string code, string clientId, string clientSecret, string redirectUrl, string state, string? verifier = null);

    /// <summary>
    ///     [App Access Token] Generate app access token for server-to-server communication.
    ///     Can access publicly available data and are ideal for use when user login is not required.
    /// </summary>
    /// <param name="clientId">App client ID.</param>
    /// <param name="clientSecret">App secret.</param>
    /// <returns>Returns access token response.</returns>
    Task<Result<KickAppTokenResponse>> GenerateAppAccessTokenAsync(string clientId, string clientSecret);

    /// <summary>
    ///     Refresh the access token using the refresh token.
    /// </summary>
    /// <param name="refreshToken">Valid refresh token.</param>
    /// <param name="clientId">App Client ID.</param>
    /// <param name="clientSecret">App secret.</param>
    /// <returns>Returns refreshed access token.</returns>
    Task<Result<KickTokenResponse>> RefreshAccessTokenAsync(string refreshToken, string clientId, string clientSecret);

    /// <summary>
    ///     Revoke Access Token.
    /// </summary>
    /// <param name="tokenToRevoke">Access token to revoke.</param>
    /// <returns>Returns true if successfully revoked.</returns>
    Task<Result<bool>> RevokeAccessTokenAsync(string tokenToRevoke);

    /// <summary>
    ///     Revoke Refresh token.
    /// </summary>
    /// <param name="tokenToRevoke">Refresh token to revoke.</param>
    /// <returns>Returns true if successfully revoked.</returns>
    Task<Result<bool>> RevokeRefreshTokenAsync(string tokenToRevoke);

    /// <summary>
    /// Revoke token
    /// </summary>
    /// <param name="tokenToRevoke">Token to revoke.</param>
    /// <param name="isAccessToken">True if token is an access token.</param>
    /// <returns>Returns true if successfully revoked.</returns>
    Task<Result<bool>> RevokeTokenAsync(string tokenToRevoke, bool isAccessToken);
}
