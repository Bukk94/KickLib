using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Contains information about positions of emotes in a message.
/// </summary>
public class Positions
{
    /// <summary>
    ///     Start index of the emote in the message.
    /// </summary>
    [JsonProperty(PropertyName = "s")]
    public int Start { get; set; }
    
    /// <summary>
    ///     End index of the emote in the message.
    /// </summary>
    [JsonProperty(PropertyName = "e")]
    public int End { get; set; }
}