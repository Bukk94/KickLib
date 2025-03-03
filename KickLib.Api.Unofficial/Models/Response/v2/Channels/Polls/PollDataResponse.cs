using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Polls
{
    public class PollDataResponse
    {
        public string Title { get; set; }

        public int Duration { get; set; }
    
        [JsonProperty(PropertyName = "result_display_duration")]
        public int ResultDisplayDuration { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        public int Remaining { get; set; }

        [JsonProperty(PropertyName = "has_voted")]
        public bool HasVoted { get; set; }

        [JsonProperty(PropertyName = "voted_option_id")]
        public int VotedOptionId { get; set; }

        public ICollection<OptionsResponse> Options { get; set; }
    }
}