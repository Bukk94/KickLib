namespace KickLib.Models.v1.Categories;

/// <summary>
///     Represents a category.
/// </summary>
public class CategoryResponse
{
    /// <summary>
    ///     Unique identifier of the category.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Name of the category.
    /// </summary>
    public required string Name { get; set; }
    
    /// <summary>
    ///     Thumbnail image URL of the category.
    /// </summary>
    public required string Thumbnail { get; set; }
}