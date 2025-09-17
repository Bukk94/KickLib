using KickLib.Client.Models.Events.Chatroom;

namespace KickLib.Client.Models.Events.Channel.Gifts;

public class KicksGiftedEvent
{
    public string Message { get; set; } = string.Empty;
    public MessageSender Sender { get; set; } = new();
    public GiftDetails Gift { get; set; } = new();
}

public class GiftDetails
{
    public string GiftId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
    public int CharacterLimit { get; set; }
    public int PinnedTime { get; set; }
}
