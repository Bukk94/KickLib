using KickLib.Client.Models.Events.Channel;

namespace KickLib.Client.Models.Args;

public class FollowersUpdatedEventArgs : EventArgs
{
    public required FollowersUpdatedEvent Data { get; set; }
}