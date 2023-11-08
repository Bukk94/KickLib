namespace KickLib.Models.Response.v2.Clips;

public class ClipsResponse
{
    public ICollection<ClipResponse> Clips { get; set; }
    
    public string NextCursor { get; set; }
}