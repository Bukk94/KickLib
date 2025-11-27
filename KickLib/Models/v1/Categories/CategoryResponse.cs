using Newtonsoft.Json;

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
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    ///     Thumbnail image URL of the category.
    /// </summary>
    public string Thumbnail { get; set; } = string.Empty;

    /// <summary>
    ///     Tags associated with the category.
    /// </summary>
    /// <remarks>
    ///     Only returned when getting specific category by ID!
    /// </remarks>
    public ICollection<string>? Tags { get; set; }
    
    /// <summary>
    ///     Number of viewers currently watching streams in this category.
    /// </summary>
    /// <remarks>
    ///     Only returned when getting specific category by ID!
    /// </remarks>
    [JsonProperty(PropertyName = "viewer_count")]
    public int? ViewerCount { get; set; }
}