using Newtonsoft.Json;

namespace KickLib.Models.v1.Auth;

/// <summary>
///     A response from the Kick API containing the public key.
/// </summary>
public class PublicKeyResponse
{
    /// <summary>
    ///     Public key used for signing.
    /// </summary>
    [JsonProperty(PropertyName = "public_key")]
    public string PublicKey { get; set; } = string.Empty;
}