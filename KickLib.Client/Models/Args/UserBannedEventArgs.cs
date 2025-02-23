using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class UserBannedEventArgs : EventArgs
{
    public required UserBannedEvent Data { get; set; }
}