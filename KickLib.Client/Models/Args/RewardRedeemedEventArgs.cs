using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Args;

public class RewardRedeemedEventArgs : EventArgs
{
    public RewardRedeemedEvent Data { get; set; } = new();
}
