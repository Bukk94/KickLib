using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class SubscriptionEventArgs : EventArgs
{
    public required SubscriptionEvent Data { get; set; }
}