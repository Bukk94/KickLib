using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v2.Messages;

public class SendMessageResponse
{
    public string Id { get; set; } = string.Empty;

    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }
    
    public string Content { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;
    
    [JsonProperty(PropertyName = "created_at")]
    public DateTime CreatedAt { get; set; }

    public MessageSenderResponse Sender { get; set; }
}