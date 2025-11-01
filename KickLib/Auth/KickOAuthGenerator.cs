using System.Net;
using System.Security.Cryptography;
using System.Text;
using KickLib.Models.Errors;
using Newtonsoft.Json;

namespace KickLib.Auth;

/// <summary>
///     KickLib service for generating OAuth 2.1 authorization flow (URLs, exchanging tokens, revokation).
/// </summary>
public class KickOAuthGenerator : IKickOAuthGenerator
{
    private readonly IHttpClientFactory _clientFactory;
    
    /// <summary>
    ///     Authorization URL
    /// </summary>
    public const string AuthorizeUrl = "https://id.kick.com/oauth/authorize";

    /// <summary>
    ///     URL used for token exchange.
    /// </summary>
    public const string ExchangeTokenUrl = "https://id.kick.com/oauth/token";
    
    /// <summary>
    ///     URL used for Token revocation.
    /// </summary>
    public const string RevokeTokenUrl = "https://id.kick.com/oauth/revoke";

    /// <summary>
    ///     Creates instance of Kick OAuth Generator.
    /// </summary>
    /// <param name="clientFactory">HTTP Client Factory for HTTP requests.</param>
    public KickOAuthGenerator(IHttpClientFactory? clientFactory = null)
    {
        _clientFactory = clientFactory ?? new KickLibHttpClientFactory();
    }
    
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
    public Uri GetAuthorizationUri(
        string redirectUri,
        string clientId,
        ICollection<string> scopes,
        out string verifier,
        string? state = null)
    {
        if (string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));
        }
        if (string.IsNullOrEmpty(redirectUri))
        {
            throw new ArgumentException("Redirect URI cannot be null or empty.", nameof(redirectUri));
        }
        if (scopes is null)
        {
            throw new ArgumentNullException(nameof(scopes), "Scopes cannot be null.");
        }

        if (scopes.Count == 0)
        {
            throw new ArgumentException("Scopes cannot be empty (select at least one of `KickScopes`).", nameof(scopes));
        }

        var scope = string.Join(" ", scopes.Distinct());

        verifier = GenerateCodeVerifier();
        if (string.IsNullOrWhiteSpace(state))
        {
            state = GenerateState(verifier);
        }

        var challenge = GenerateChallengeCode(verifier);

        var url = new StringBuilder(AuthorizeUrl)
            .Append("?client_id=").Append(clientId)
            .Append("&response_type=code")
            .Append("&redirect_uri=").Append(redirectUri)
            .Append("&state=").Append(state)
            .Append("&scope=").Append(scope)
            .Append("&code_challenge=").Append(challenge)
            .Append("&code_challenge_method=S256")
            .ToString();

        return new Uri(url);
    }

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
    public async Task<Result<KickTokenResponse>> ExchangeCodeForTokenAsync(
        string code,
        string clientId,
        string clientSecret,
        string redirectUrl,
        string state,
        string? verifier = null)
    {
        if (string.IsNullOrEmpty(code))
        {
            throw new ArgumentException("Code cannot be null or empty.", nameof(code));
        }
        if (string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));
        }
        if (string.IsNullOrEmpty(state))
        {
            throw new ArgumentException("State cannot be null or empty.", nameof(state));
        }
        if (string.IsNullOrEmpty(redirectUrl))
        {
            throw new ArgumentException("Redirect URL cannot be null or empty.", nameof(redirectUrl));
        }

        var client = GetClient();

        if (string.IsNullOrWhiteSpace(verifier))
        {
            verifier = DecodeState(state);
        }

        var data = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("code", code),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("redirect_uri", redirectUrl),
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            // This is the PKCE part. It's the ORIGINAL generated code before it was sha256 hashed.
            // This is what proves to kick that you are the one that started the original auth request
            new KeyValuePair<string, string>("code_verifier", verifier)
        ]);

        var response = await client.PostAsync(
            ExchangeTokenUrl,
            data).ConfigureAwait(false);

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var message =
                $"ExchangeCodeForToken call returned non-Ok response. Reason: {response.ReasonPhrase}. Status: {response.StatusCode}. Data: {content}";
            return Result.Fail(message);
        }

        try
        {
            var deserialized = JsonConvert.DeserializeObject<KickTokenResponse>(content);
            
            return deserialized is not null 
                ? Result.Ok(deserialized) 
                : Result.Fail($"ExchangeCodeForToken failed due to deserialization issue. Payload: {content}");
        }
        catch (Exception ex)
        {
            var message = $"ExchangeCodeForToken failed to deserialize response: {content}\nException:{ex}";
            return Result.Fail(message);
        }
    }
    
    /// <summary>
    ///     [App Access Token] Generate app access token for server-to-server communication.
    ///     Can access publicly available data and are ideal for use when user login is not required.
    /// </summary>
    /// <param name="clientId">App client ID.</param>
    /// <param name="clientSecret">App secret.</param>
    /// <returns>Returns access token response.</returns>
    public async Task<Result<KickAppTokenResponse>> GenerateAppAccessTokenAsync(
        string clientId,
        string clientSecret)
    {
        if (string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));
        }

        var client = GetClient();

        var data = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("grant_type", "client_credentials")
        ]);

        var response = await client.PostAsync(
            ExchangeTokenUrl,
            data).ConfigureAwait(false);

        var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var message =
                $"ExchangeCodeForToken call returned non-Ok response. Reason: {response.ReasonPhrase}. Status: {response.StatusCode}. Data: {content}";
            return Result.Fail(message);
        }

        try
        {
            var deserialized = JsonConvert.DeserializeObject<KickAppTokenResponse>(content);
            
            return deserialized is not null 
                ? Result.Ok(deserialized) 
                : Result.Fail($"GenerateAppAccessTokenAsync failed due to deserialization issue. Payload: {content}");
        }
        catch (Exception ex)
        {
            var message = $"GenerateAppAccessTokenAsync failed to deserialize response: {content}\nException:{ex}";
            return Result.Fail(message);
        }
    }

    /// <summary>
    ///     Refresh the access token using the refresh token.
    /// </summary>
    /// <param name="refreshToken">Valid refresh token.</param>
    /// <param name="clientId">App Client ID.</param>
    /// <param name="clientSecret">App secret.</param>
    /// <returns>Returns refreshed access token.</returns>
    public async Task<Result<KickTokenResponse>> RefreshAccessTokenAsync(
        string refreshToken,
        string clientId,
        string clientSecret)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));
        }
        if (string.IsNullOrEmpty(clientId))
        {
            throw new ArgumentException("Client ID cannot be null or empty.", nameof(clientId));
        }
        if (string.IsNullOrEmpty(clientSecret))
        {
            throw new ArgumentException("Client secret cannot be null or empty.", nameof(clientSecret));
        }
        
        var client = GetClient();

        var data = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("refresh_token", refreshToken),
            new KeyValuePair<string, string>("client_id", clientId),
            new KeyValuePair<string, string>("client_secret", clientSecret),
            new KeyValuePair<string, string>("grant_type", "refresh_token")
        ]);

        var response = await client.PostAsync(
            ExchangeTokenUrl,
            data).ConfigureAwait(false);

        var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            var errorMessage =
                $"RefreshAccessTokenAsync call returned non-Ok response. Reason: {response.ReasonPhrase}. Status: {response.StatusCode}. Data: {message}";
            return Result.Fail(errorMessage).WithError(new KickLibHttpResponseError(errorMessage, response));
        }

        var deserializeResponse = JsonConvert.DeserializeObject<KickTokenResponse>(message);
        return deserializeResponse is not null 
            ? Result.Ok(deserializeResponse) 
            : Result.Fail("Refresh token failed due to deserialization issue. Payload: " + message)
                .WithError(new KickLibHttpResponseError(message, response));
    }

    /// <summary>
    ///     Revoke Access Token.
    /// </summary>
    /// <param name="tokenToRevoke">Access token to revoke.</param>
    /// <returns>Returns true if successfully revoked.</returns>
    public Task<Result<bool>> RevokeAccessTokenAsync(string tokenToRevoke) => RevokeTokenAsync(tokenToRevoke, true);
    
    /// <summary>
    ///     Revoke Refresh token.
    /// </summary>
    /// <param name="tokenToRevoke">Refresh token to revoke.</param>
    /// <returns>Returns true if successfully revoked.</returns>
    public Task<Result<bool>> RevokeRefreshTokenAsync(string tokenToRevoke) => RevokeTokenAsync(tokenToRevoke, false);
    
    /// <summary>
    ///     Revoke access/refresh token.
    /// </summary>
    /// <param name="tokenToRevoke">Token to revoke.</param>
    /// <param name="isAccessToken">Is passed token an Access Token?</param>
    /// <returns>Returns true if successfully revoked.</returns>
    public async Task<Result<bool>> RevokeTokenAsync(
        string tokenToRevoke,
        bool isAccessToken)
    {
        if (string.IsNullOrEmpty(tokenToRevoke))
        {
            throw new ArgumentException("Token to revoke cannot be null or empty.", nameof(tokenToRevoke));
        }
        
        var client = GetClient();

        var hintType = isAccessToken ? "access_token" : "refresh_token";
        var data = new FormUrlEncodedContent(
        [
            new KeyValuePair<string, string>("token", tokenToRevoke),
            new KeyValuePair<string, string>("token_hint_type", hintType)
        ]);

        var response = await client.PostAsync(
            RevokeTokenUrl,
            data).ConfigureAwait(false);

        var message = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

        return response.StatusCode == HttpStatusCode.OK
            ? Result.Ok()
            : Result.Fail(message);
    }

    private static string GenerateChallengeCode(string verifier)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(verifier));
        return Convert.ToBase64String(hash)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    /// <summary>
    ///     The verifier system is used to "prove" that the request for authorization was 
    ///     started by your application, and later that the code exchange was also by your application.
    /// </summary>
    private static string GenerateCodeVerifier()
    {
        var buffer = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(buffer);

        return Convert.ToBase64String(buffer)
            .Replace("+", "-")
            .Replace("/", "_")
            .Replace("=", "");
    }

    private static string GenerateState(string codeVerifier)
    {
        // Using verifier as state is not recommended and unsafe.
        var json = JsonConvert.SerializeObject(new KickVerifier { CodeVerifier = codeVerifier });
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        return Convert.ToBase64String(jsonBytes);
    }

    private static string DecodeState(string state)
    {
        try
        {
            state = WebUtility.UrlDecode(state);
            var jsonBytes = Convert.FromBase64String(state);
            var json = Encoding.UTF8.GetString(jsonBytes);
            var data = JsonConvert.DeserializeObject<KickVerifier>(json);

            return data!.CodeVerifier;
        }
        catch
        {
            return string.Empty;
        }
    }
    
    private HttpClient GetClient() => _clientFactory.CreateClient(HttpConstants.HttpClientName);
}