using System.Text.RegularExpressions;

namespace KickLib.Models.v1.ChannelRewards;

/// <summary>
///     Request to update an existing channel reward.
/// </summary>
public class UpdateChannelRewardRequest
{
    private string? _description;
    private int? _cost;
    private string? _title;
    private string? _backgroundColor;

    /// <summary>
    ///     Title of the reward.
    /// </summary>
    public string? Title
    {
        get => _title;
        set
        {
            if (value != null && value.Length > 50)
            {
                throw new ArgumentOutOfRangeException(nameof(Title), "Title cannot be longer than 50 characters.");
            }

            _title = value;
        }
    }

    /// <summary>
    ///     Cost of the reward in channel points.
    /// </summary>
    public int? Cost
    {
        get => _cost;
        set
        {
            if (value != null && value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(Cost), "Cost must be greater than or equal to 1.");
            }

            _cost = value;
        }
    }

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
    public bool? IsEnabled { get; set; }
    
    /// <summary>
    ///     Indicates whether the reward is paused.
    /// </summary>
    public bool? IsPaused { get; set; }

    /// <summary>
    ///     Indicates whether user input is required when redeeming the reward.
    /// </summary>
    public bool IsUserInputRequired { get; set; }

    /// <summary>
    ///     Indicates whether redemptions of this reward should skip the request queue.
    /// </summary>
    public bool ShouldRedemptionsSkipRequestQueue { get; set; }
}