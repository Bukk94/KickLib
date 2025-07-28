using KickLib.Client.Models.Events.Livestream;

namespace KickLib.Client.Models.Args;

public class StreamStateChangedArgs : EventArgs
{
    public bool IsLive { get; set; }

    public int ChannelId { get; set; }
    
    public LivestreamChangedEvent Data { get; set; } = new();
}