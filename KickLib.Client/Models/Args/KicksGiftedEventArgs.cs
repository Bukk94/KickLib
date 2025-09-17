using KickLib.Client.Models.Events.Channel.Gifts;

namespace KickLib.Client.Models.Args;

public class KicksGiftedEventArgs : EventArgs
{
    public KicksGiftedEvent Data { get; set; } = new();
}
