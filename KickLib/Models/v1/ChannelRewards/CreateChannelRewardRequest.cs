using System.Text.RegularExpressions;

namespace KickLib.Models.v1.ChannelRewards;

/// <summary>
///     Request to create a new channel reward.
/// </summary>
public class CreateChannelRewardRequest
{
    private string? _description;
    private string? _backgroundColor;
    
    /// <summary>
    ///     Creates a new instance of <see cref="CreateChannelRewardRequest"/>.
    /// </summary>
    /// <param name="cost">Cost of the reward in channel points (must be 1 or greated).</param>
    /// <param name="title">Title of the reward (50 characters max).</param>
    /// <exception cref="ArgumentNullException">Thrown when the title is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the title is longer than 50 characters. </exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the cost is less than 1.</exception>
    public CreateChannelRewardRequest(int cost, string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException(nameof(title));
        }
        
        if (title.Length > 50)
        {
            throw new ArgumentOutOfRangeException(nameof(title), "Title cannot be longer than 50 characters.");
        }
        
        if (cost < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(cost), "Cost must be greater than or equal to 1.");
        }
        
        Cost = cost;
        Title = title;
    }
    
    /// <summary>
    ///     Title of the reward.
    /// </summary>
    public string Title { get; }
    
    /// <summary>
    ///     Cost of the reward in channel points.
    /// </summary>
    public int Cost { get; }
    
    /// <summary>
    ///     Background color of the reward in hexadecimal format.
    /// </summary>
    public string? BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            if (value != null && !Regex.IsMatch(value, "^#([0-9A-Fa-f]{6})$"))
            {
                throw new ArgumentOutOfRangeException(nameof(BackgroundColor), "BackgroundColor must be in hexadecimal format (e.g., #00e701).");
            }
                
            _backgroundColor = value;
        }
    }
    
    /// <summary>
    ///     Description of the reward (200 characters max).
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the description is longer than 200 characters.</exception>
    public string? Description
    {
        get => _description;
        set
        {
            if (value != null && value.Length > 200)
            {
                throw new ArgumentOutOfRangeException(nameof(Description), "Description cannot be longer than 200 characters.");
            }
            
            _description = value;
        }
    }

    /// <summary>
    ///     Indicates whether the reward is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    ///     Indicates whether user input is required when redeeming the reward.
    /// </summary>
    public bool IsUserInputRequired { get; set; }

    /// <summary>
    ///     Indicates whether redemptions of this reward should skip the request queue.
    /// </summary>
    public bool ShouldRedemptionsSkipRequestQueue { get; set; }
}
