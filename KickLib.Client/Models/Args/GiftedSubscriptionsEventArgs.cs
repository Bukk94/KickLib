using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class GiftedSubscriptionsEventArgs : EventArgs
{
    public required GiftedSubscriptionsEvent Data { get; set; }
}