using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     User's identity details.
/// </summary>
public class UserIdentity
{
    /// <summary>
    ///     Color for username in hex format (e.g. #FF5733).
    /// </summary>
    [JsonProperty(PropertyName = "username_color")]
    public required string UsernameColor { get; set; }

    /// <summary>
    ///     A list of badges user has.
    /// </summary>
    public ICollection<Badge> Badges { get; set; } = new List<Badge>();
}