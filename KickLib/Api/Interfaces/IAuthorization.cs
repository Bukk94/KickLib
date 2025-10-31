using KickLib.Auth;
using KickLib.Models.v1.Auth;

namespace KickLib.Api.Interfaces;

/// <summary>
///     Authorization operations.
/// </summary>
public interface IAuthorization
{
    /// <summary>
    ///     Get information about the token that is passed in via the Authorization header.
    ///     This function is implements part of the on the OAuth 2.0 spec for token introspection.
    ///     When active=false there is no additional information added in the response.
    /// </summary>
    /// <remarks>
    ///     Find the full spec here: https://datatracker.ietf.org/doc/html/rfc7662
    /// </remarks>
    /// <param name="accessToken">Access token to be used for this request. If null, token from <see cref="ApiSettings"/> will be used.</param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<TokenIntrospectResponse>> IntrospectTokenAsync(string? accessToken = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieve the public key used for verifying signatures.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    Task<Result<PublicKeyResponse>> GetPublicKeyAsync(CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get app access token based on <see cref="ApiSettings.ClientId"/> and <see cref="ApiSettings.ClientSecret"/> API settings.
    /// </summary>
    /// <returns>Returns access token for the application, used for public endpoints.</returns>
    Task<Result<KickAppTokenResponse>> GetAppAccessTokenAsync();
    
    /// <summary>
    ///     Get app access token.
    /// </summary>
    /// <param name="clientId">Application client ID.</param>
    /// <param name="clientSecret">Application secret.</param>
    /// <returns>Returns access token for the application, used for public endpoints.</returns>
    Task<Result<KickAppTokenResponse>> GetAppAccessTokenAsync(string clientId, string clientSecret);
}
