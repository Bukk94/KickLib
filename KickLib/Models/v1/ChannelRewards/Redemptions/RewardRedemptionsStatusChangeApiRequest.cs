namespace KickLib.Models.v1.ChannelRewards.Redemptions;

internal class RewardRedemptionsStatusChangeApiRequest
{
    public ICollection<string> Ids { get; set; } = [];
}