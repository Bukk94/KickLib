namespace KickLib.Client.Models.Events.Chatroom;

public class SenderIdentity
{
    public string Color { get; set; } = string.Empty;

    public ICollection<Badge> Badges { get; set; } = new List<Badge>();
}