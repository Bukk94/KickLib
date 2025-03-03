using KickLib.Models.v1.Auth;
using Microsoft.Extensions.Logging;

namespace KickLib.Api;

public class Authorization : ApiBase
{
    // TODO: OAuth 2.1
    private const string PublicKeyApiUrlPart = "public-key";
    private const string IntrospectApiUrlPart = "token/introspect";

    public Authorization(ApiSettings settings, ILogger logger) : base(settings, logger)
    {
    }
    
    /// <summary>
    ///     Get information about the token that is passed in via the Authorization header.
    ///     This function is implements part of the on the OAuth 2.0 spec for token introspection.
    ///     When active=false there is no additional information added in the response.
    /// </summary>
    /// <remarks>
    ///     Find the full spec here: https://datatracker.ietf.org/doc/html/rfc7662
    /// </remarks>
    public Task<Result<TokenIntrospectResponse>> IntrospectTokenAsync(string? accessToken = null)
    {
        // v1/token/introspect
        return PostAsync<TokenIntrospectResponse>(IntrospectApiUrlPart, ApiVersion.v1, accessToken);
    }
    
    /// <summary>
    ///     Retrieve the public key used for verifying signatures.
    /// </summary>
    public Task<Result<PublicKeyResponse>> GetPublicKeyAsync(string? accessToken = null)
    {
        // v1/public-key
        return GetAsync<PublicKeyResponse>(PublicKeyApiUrlPart, ApiVersion.v1, null, accessToken);
    }
}