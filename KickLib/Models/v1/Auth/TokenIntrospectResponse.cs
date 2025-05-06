using Newtonsoft.Json;

namespace KickLib.Models.v1.Auth;

/// <summary>
///     Introspect response data.
/// </summary>
public class TokenIntrospectResponse
{
    /// <summary>
    ///     Is token active?
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    ///     Client who granted the the token.
    ///     Is null if token is not active.
    /// </summary>
    [JsonProperty(PropertyName = "client_id")]
    public string? ClientId { get; set; }

    /// <summary>
    ///     Seconds until token expiration.
    ///     Is null if token is not active.
    /// </summary>
    [JsonProperty(PropertyName = "exp")]
    public int? ExpiresIn { get; set; }

    /// <summary>
    ///     Scope of the token.
    ///     Is null if token is not active.
    /// </summary>
    public string? Scope { get; set; }

    /// <summary>
    ///     Get token scopes.
    /// </summary>
    public ICollection<string> GetScopes()
    {
        return !string.IsNullOrWhiteSpace(Scope)
            ? Scope.Split(' ')
            : [];
    }
}