using KickLib.Client.Models.Events.Chatroom.Pins;

namespace KickLib.Client.Models.Args;

public class PinnedMessageDeletedEventArgs : EventArgs
{
    public required PinnedMessageDeletedEvent Data { get; set; }
}