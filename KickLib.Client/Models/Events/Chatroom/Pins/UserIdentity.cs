namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class UserIdentity
{
    public string Color { get; set; } = string.Empty;
    
    public ICollection<PinBadge> Badges { get; set; } = new List<PinBadge>();
}