using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class StreamHostEventArgs : EventArgs
{
    public StreamHostEvent Data { get; set; } = new();
}