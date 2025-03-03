using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class PlanResponse
    {
        public int Id { get; set; }
    
        [JsonProperty(PropertyName = "channel_id")]
        public int ChannelId { get; set; }
    
        [JsonProperty(PropertyName = "stripe_plan_id")]
        public string StripePlanId { get; set; }

        public double Amount { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}