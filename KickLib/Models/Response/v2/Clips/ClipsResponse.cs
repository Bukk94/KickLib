namespace KickLib.Models.Response.v2.Clips;

public class ClipsResponse
{
    public ICollection<ClipResponse> Clips { get; set; }
    
    public int NextCursor { get; set; }
}