using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class GiftedSubscriptionsEventArgs : EventArgs
{
    public GiftedSubscriptionsEvent Data { get; set; } = new();
}