using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class MessageDeletedEventArgs : EventArgs
{
    public required MessageDeletedEvent Data { get; set; }
}