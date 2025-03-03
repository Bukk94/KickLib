namespace KickLib.Api.Unofficial.Models.Response.v2.Categories
{
    public class CategoryClipsResponse
    {
        public ICollection<CategoryClipResponse> Clips { get; set; }
    
        public string NextCursor { get; set; }
    }
}