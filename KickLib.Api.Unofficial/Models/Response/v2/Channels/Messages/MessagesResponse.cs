using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Messages
{
    public class MessagesResponse
    {
        public ICollection<MessageResponse> Messages { get; set; }

        public string Cursor { get; set; }

        [JsonProperty(PropertyName = "pinned_message")]
        public PinnedMessageResponse PinnedMessage { get; set; }
    }
}