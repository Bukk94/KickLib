using KickLib.Api.Interfaces;
using KickLib.Auth;
using KickLib.Models.v1.Auth;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

/// <inheritdoc cref="IAuthorization" />
public class Authorization : ApiBase, IAuthorization
{
    private readonly ApiSettings _settings;
    private readonly IKickOAuthGenerator _kickOAuthGenerator;
    private const string PublicKeyApiUrlPart = "public-key";
    private const string IntrospectApiUrlPart = "token/introspect";

    /// <inheritdoc />
    public Authorization(ApiSettings settings, IKickOAuthGenerator oauthGenerator, IHttpClientFactory clientFactory, ILogger<Authorization> logger) 
        : base(settings, oauthGenerator, clientFactory, logger)
    {
        _kickOAuthGenerator = oauthGenerator;
        _settings = settings;
    }
    
    /// <summary>
    ///     Get information about the token that is passed in via the Authorization header.
    ///     This function is implements part of the on the OAuth 2.0 spec for token introspection.
    ///     When active=false there is no additional information added in the response.
    /// </summary>
    /// <remarks>
    ///     Find the full spec here: https://datatracker.ietf.org/doc/html/rfc7662
    /// </remarks>
    public Task<Result<TokenIntrospectResponse>> IntrospectTokenAsync(
        string? accessToken = null,
        CancellationToken cancellationToken = default)
    {
        // v1/token/introspect
        return PostAsync<TokenIntrospectResponse>(IntrospectApiUrlPart, ApiVersion.v1, accessToken, cancellationToken);
    }
    
    /// <summary>
    ///     Retrieve the public key used for verifying signatures.
    /// </summary>
    public Task<Result<PublicKeyResponse>> GetPublicKeyAsync(CancellationToken cancellationToken = default)
    {
        // v1/public-key
        return GetAsync<PublicKeyResponse>(PublicKeyApiUrlPart, ApiVersion.v1, cancellationToken: cancellationToken);
    }
    
    /// <summary>
    ///     Get app access token based on <see cref="ApiSettings.ClientId"/> and <see cref="ApiSettings.ClientSecret"/> API settings.
    /// </summary>
    /// <returns>Returns access token for the application, used for public endpoints.</returns>
    public async Task<Result<KickAppTokenResponse>> GetAppAccessTokenAsync()
    {
        if (string.IsNullOrWhiteSpace(_settings.ClientId))
        {
            return Result.Fail("Client ID is required. Set it in the API settings.");
        }
        
        if (string.IsNullOrWhiteSpace(_settings.ClientSecret))
        {
            return Result.Fail("Client Secret is required. Set it in the API settings.");
        }
        
        return await GetAppAccessTokenAsync(_settings.ClientId, _settings.ClientSecret).ConfigureAwait(false);
    }
    
    /// <summary>
    ///     Get app access token.
    /// </summary>
    /// <returns>Returns access token for the application, used for public endpoints.</returns>
    public async Task<Result<KickAppTokenResponse>> GetAppAccessTokenAsync(
        string clientId,
        string clientSecret)
    {
        if (string.IsNullOrWhiteSpace(_settings.ClientId))
        {
            return Result.Fail("ClientId is required");
        }
        
        if (string.IsNullOrWhiteSpace(_settings.ClientSecret))
        {
            return Result.Fail("ClientSecret is required");
        }

        return await _kickOAuthGenerator.GenerateAppAccessTokenAsync(clientId, clientSecret).ConfigureAwait(false);
    }
}