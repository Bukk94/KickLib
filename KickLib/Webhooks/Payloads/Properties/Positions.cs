using Newtonsoft.Json;

namespace KickLib.Webhooks.Payloads;

public class Positions
{
    [JsonProperty(PropertyName = "s")]
    public int Start { get; set; }
    
    [JsonProperty(PropertyName = "e")]
    public int End { get; set; }
}