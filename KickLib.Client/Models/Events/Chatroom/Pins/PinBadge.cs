namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinBadge
{
    public required string Type { get; set; }
    
    public required string Text { get; set; }
    
    public int? Count { get; set; }

    public bool? Active { get; set; }
}