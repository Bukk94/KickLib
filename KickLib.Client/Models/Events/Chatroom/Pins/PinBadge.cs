namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinBadge
{
    public string Type { get; set; } = string.Empty;
    
    public string Text { get; set; } = string.Empty;
    
    public int? Count { get; set; }

    public bool? Active { get; set; }
}