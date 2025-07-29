namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Represents a category.
/// </summary>
public class Category
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
}