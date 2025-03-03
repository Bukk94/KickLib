namespace KickLib.Models.v1.Categories;

public class CategoryResponse
{
    public int Id { get; set; }

    public required string Name { get; set; }
    
    public required string Thumbnail { get; set; }
}