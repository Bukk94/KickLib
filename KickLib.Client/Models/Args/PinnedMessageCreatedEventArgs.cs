using KickLib.Client.Models.Events.Chatroom.Pins;

namespace KickLib.Client.Models.Args;

public class PinnedMessageCreatedEventArgs : EventArgs
{
    public PinnedMessageCreatedEvent Data { get; set; } = new();
}