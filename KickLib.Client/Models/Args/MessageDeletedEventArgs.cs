using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class MessageDeletedEventArgs : EventArgs
{
    public MessageDeletedEvent Data { get; set; } = new();
}