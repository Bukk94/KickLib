using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class ChatMessageEventArgs : EventArgs
{
    public required ChatMessageEvent Data { get; set; }
}