using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Channels
{
    public class ChatroomResponse
    {
        public int Id { get; set; }

        [JsonProperty(PropertyName = "chatable_type")]
        public string ChatableType { get; set; }
    
        [JsonProperty(PropertyName = "channel_id")]
        public int ChannelId { get; set; }
    
        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; set; }
    
        [JsonProperty(PropertyName = "updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "chat_mode_old")]
        public string ChatModeOld { get; set; }
    
        // TODO: Once discovered all possible values, this can be enum
        [JsonProperty(PropertyName = "followers_only")]
        public string FollowersOnly { get; set; }
    
        [JsonProperty(PropertyName = "slow_mode")]
        public bool SlowMode { get; set; }
    
        [JsonProperty(PropertyName = "chatable_id")]
        public int ChatableId { get; set; }
    
        [JsonProperty(PropertyName = "followers_mode")]
        public bool FollowersMode { get; set; }
    
        [JsonProperty(PropertyName = "subscribers_mode")]
        public bool SubscribersMode { get; set; }
    
        [JsonProperty(PropertyName = "emotes_mode")]
        public bool EmotesMode { get; set; }
    
        [JsonProperty(PropertyName = "message_interval")]
        public int MessageInterval { get; set; }
    
        [JsonProperty(PropertyName = "following_min_duration")]
        public int FollowingMinDuration { get; set; }
    }
}