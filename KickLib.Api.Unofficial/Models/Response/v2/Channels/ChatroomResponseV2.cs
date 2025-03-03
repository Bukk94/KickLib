using KickLib.Api.Unofficial.Models.Response.v2.Channels.Chatroom;
using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels
{
    public class ChatroomResponseV2
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "slow_mode")]
        public SlowModeResponse SlowMode { get; set; }
    
        [JsonProperty(PropertyName = "subscribers_mode")]
        public SubscribersModeResponse SubscribersMode { get; set; }
    
        [JsonProperty(PropertyName = "followers_mode")]
        public FollowersModeResponse FollowersMode { get; set; }
    
        [JsonProperty(PropertyName = "emotes_mode")]
        public EmotesModeResponse EmotesMode { get; set; }
    
        [JsonProperty(PropertyName = "advanced_bot_protection")]
        public AdvancedBotProtectionResponse AdvancedBotProtection { get; set; }
    
        // TODO: pinned_message
    }
}