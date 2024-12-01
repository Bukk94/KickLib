using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class GiftedSubscriptionsEvent
{
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    [JsonProperty(PropertyName = "gifted_usernames")]
    public ICollection<string> GiftedUsernames { get; set; }
    
    [JsonProperty(PropertyName = "gifter_username")]
    public string GifterUsername { get; set; }
    
    [JsonProperty(PropertyName = "gifter_total")]
    public int GifterTotal { get; set; }
}