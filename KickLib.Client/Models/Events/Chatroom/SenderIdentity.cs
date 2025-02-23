namespace KickLib.Client.Models.Events.Chatroom;

public class SenderIdentity
{
    public required string Color { get; set; }

    public ICollection<Badge> Badges { get; set; } = new List<Badge>();
}