namespace KickLib.Models.v1.Categories;

/// <summary>
///     Request object for retrieving categories.
/// </summary>
public class GetCategoriesRequest
{
    /// <summary>
    ///     Maximum number of categories to retrieve.
    /// </summary>
    public int? Limit { get; set; }
    
    /// <summary>
    ///     Cursor value to get next page of results.
    /// </summary>
    public string? Cursor { get; set; }
    
    /// <summary>
    ///     Names of the categories to filter by.
    /// </summary>
    public ICollection<string>? Names { get; set; }
    
    /// <summary>
    ///     Tags to filter categories by.
    /// </summary>
    public ICollection<string>? Tags { get; set; }
    
    /// <summary>
    ///     IDs of the categories to filter by.
    /// </summary>
    public ICollection<int>? Ids { get; set; }
}