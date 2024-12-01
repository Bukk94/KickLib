namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinBadge
{
    public string Type { get; set; }
    
    public string Text { get; set; }
    
    public int? Count { get; set; }

    public bool? Active { get; set; }
}