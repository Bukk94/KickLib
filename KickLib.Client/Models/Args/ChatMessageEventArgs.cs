using KickLib.Client.Models.Events;
using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class ChatMessageEventArgs : EventArgs
{
    public ChatMessageEvent Data { get; set; }
}