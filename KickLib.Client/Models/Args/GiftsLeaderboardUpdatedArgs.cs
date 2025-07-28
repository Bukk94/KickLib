using KickLib.Client.Models.Events.Channel.Gifts;

namespace KickLib.Client.Models.Args;

public class GiftsLeaderboardUpdatedArgs : EventArgs
{
    public GiftsLeaderboardUpdatedEvent Data { get; set; } = new();
}