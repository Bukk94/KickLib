using Newtonsoft.Json;

namespace KickLib.Models.v1.ChannelRewards;

internal class UpdateChannelRewardApiRequest
{
    [JsonProperty(PropertyName = "background_color")]
    public string? BackgroundColor { get; set; }
    
    public int? Cost { get; set; }
    
    public string? Description { get; set; }
    
    [JsonProperty(PropertyName = "is_enabled")]
    public bool? IsEnabled { get; set; }
    
    [JsonProperty(PropertyName = "is_paused")]
    public bool? IsPaused { get; set; }
    
    [JsonProperty(PropertyName = "is_user_input_required")]
    public bool? IsUserInputRequired { get; set; }
    
    [JsonProperty(PropertyName = "should_redemptions_skip_request_queue")]
    public bool? ShouldRedemptionsSkipRequestQueue { get; set; }

    public string? Title { get; set; }
    
    internal static UpdateChannelRewardApiRequest FromRequest(UpdateChannelRewardRequest request)
    {
        return new UpdateChannelRewardApiRequest
        {
            BackgroundColor = request.BackgroundColor,
            Cost = request.Cost,
            Description = request.Description,
            IsEnabled = request.IsEnabled,
            IsPaused = request.IsPaused,
            IsUserInputRequired = request.IsUserInputRequired,
            ShouldRedemptionsSkipRequestQueue = request.ShouldRedemptionsSkipRequestQueue,
            Title = request.Title
        };
    }
}