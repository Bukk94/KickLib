namespace KickLib.Models.v1;

/// <summary>
///     Paginated response from the Kick API.
/// </summary>
public class PaginatedResponse<TPayload>
{
    /// <summary>
    ///     The payload data of the response.
    /// </summary>
#if NET8_0_OR_GREATER
    public required TPayload Data { get; set; }
#else
    public TPayload Data { get; set; } = default!;
#endif
    
    /// <summary>
    ///     Next cursor for fetching the next page of results.
    ///     If empty or null, there are no more results.
    /// </summary>
    public string? NextCursor { get; set; }
    
    /// <summary>
    ///     Indicates if there is a next page of results.
    /// </summary>
    public bool HasNextPage => !string.IsNullOrWhiteSpace(NextCursor);
}