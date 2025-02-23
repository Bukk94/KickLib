namespace KickLib.Client.Models.Args;

public class ClientConnectedArgs : EventArgs
{
    public required string SocketId { get; set; }
}