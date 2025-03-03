using Newtonsoft.Json;

namespace KickLib.Models.v1.Auth;

public class PublicKeyResponse
{
    [JsonProperty(PropertyName = "public_key")]
    public required string PublicKey { get; set; }
}