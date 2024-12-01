namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class UserIdentity
{
    public string Color { get; set; }
    public ICollection<PinBadge> Type { get; set; }
}