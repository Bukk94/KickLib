using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class UserUnbannedEventArgs : EventArgs
{
    public required UserUnbannedEvent Data { get; set; }
}