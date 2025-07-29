using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class UserUnbannedEventArgs : EventArgs
{
    public UserUnbannedEvent Data { get; set; } = new();
}