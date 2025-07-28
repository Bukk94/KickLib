using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Contains information about emotes in a message.
/// </summary>
public class Emotes
{
    /// <summary>
    ///     ID of the emote.
    /// </summary>
    [JsonProperty(PropertyName = "emote_id")]
    public string EmoteId { get; set; } = string.Empty;

    /// <summary>
    ///     Positions of the emote in the message.
    /// </summary>
    public ICollection<Positions> Positions { get; set; } = [];
}