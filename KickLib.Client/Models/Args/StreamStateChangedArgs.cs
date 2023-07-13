using KickLib.Client.Models.Events.Livestream;

namespace KickLib.Client.Models.Args;

public class StreamStateChangedArgs : EventArgs
{
    public bool IsLive { get; set; }

    public LivestreamStartedEvent Data { get; set; }
}