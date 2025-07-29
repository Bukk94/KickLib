namespace KickLib.Client.Models.Args;

public class ClientConnectedArgs : EventArgs
{
    public string SocketId { get; set; } = string.Empty;
}