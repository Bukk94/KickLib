namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinnedMessageCreatedEvent
{
    public PinMessage Message { get; set; }
    public string Duration { get; set; }
    public PinUser PinnedBy { get; set; }
}