using Newtonsoft.Json;

namespace KickLib.Models.v1.ChannelRewards;

internal class CreateChannelRewardApiRequest
{
    [JsonProperty(PropertyName = "background_color")]
    public string? BackgroundColor { get; set; }
    
    public int Cost { get; set; }
    
    public string? Description { get; set; }
    
    [JsonProperty(PropertyName = "is_enabled")]
    public bool? IsEnabled { get; set; }
    
    [JsonProperty(PropertyName = "is_user_input_required")]
    public bool IsUserInputRequired { get; set; }
    
    [JsonProperty(PropertyName = "should_redemptions_skip_request_queue")]
    public bool? ShouldRedemptionsSkipRequestQueue { get; set; }

    public string Title { get; set; } = string.Empty;
    
    internal static CreateChannelRewardApiRequest FromRequest(CreateChannelRewardRequest request)
    {
        return new CreateChannelRewardApiRequest
        {
            BackgroundColor = request.BackgroundColor,
            Cost = request.Cost,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
            IsUserInputRequired = request.IsUserInputRequired,
            ShouldRedemptionsSkipRequestQueue = request.ShouldRedemptionsSkipRequestQueue,
            Title = request.Title
        };
    }
}