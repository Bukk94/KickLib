using Newtonsoft.Json;

namespace KickLib.Models;

/// <summary>
///     Kick API pagination information.
/// </summary>
public class Pagination
{
    /// <summary>
    ///     Next cursor for fetching the next page of results.
    ///     If empty or null, there are no more results.
    /// </summary>
    [JsonProperty(PropertyName = "next_cursor")]
    public string? NextCursor { get; set; }
}