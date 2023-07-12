namespace KickLib.Client.Models.Events.Chatroom;

public class Badge
{
    // TODO: Convert to enum when all values are known
    public string Type { get; set; }
    
    public string Text { get; set; }
    
    public int Count { get; set; }
}