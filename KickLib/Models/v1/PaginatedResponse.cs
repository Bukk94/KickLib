namespace KickLib.Models.v1;

public class PaginatedResponse<TPayload>
{
    public TPayload Data { get; set; }
    public string? NextCursor { get; set; }
}