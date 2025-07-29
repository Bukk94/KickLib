using Newtonsoft.Json;

namespace KickLib.Client.Models.Events.Chatroom;

public class GiftedSubscriptionsEvent
{
    /// <summary>
    ///     ID of the chatroom, where this event occurred.
    /// </summary>
    [JsonProperty(PropertyName = "chatroom_id")]
    public int ChatroomId { get; set; }

    /// <summary>
    ///     Usernames of users, who received gifted subscription.
    /// </summary>
    [JsonProperty(PropertyName = "gifted_usernames")]
    public ICollection<string> GiftedTo { get; set; } = new List<string>();

    /// <summary>
    ///     Number of given subscriptions.
    /// </summary>
    public int Count => GiftedTo.Count;
     
    /// <summary>
    ///     Username of the user, who gifted the subscriptions.
    /// </summary>
    [JsonProperty(PropertyName = "gifter_username")]
    public string GifterUsername { get; set; } = string.Empty;
    
    /// <summary>
    ///     Number of subscriptions this user gifted in total (not during this event, but in total). 
    /// </summary>
    [JsonProperty(PropertyName = "gifter_total")]
    public int GifterTotal { get; set; }
}