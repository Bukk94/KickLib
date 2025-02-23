using KickLib.Client.Models.Events.Channel.Gifts;

namespace KickLib.Client.Models.Args;

public class GiftsLeaderboardUpdatedArgs : EventArgs
{
    public required GiftsLeaderboardUpdatedEvent Data { get; set; }
}