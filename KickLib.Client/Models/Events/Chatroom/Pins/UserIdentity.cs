namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class UserIdentity
{
    public required string Color { get; set; }
    
    public ICollection<PinBadge> Badges { get; set; } = new List<PinBadge>();
}