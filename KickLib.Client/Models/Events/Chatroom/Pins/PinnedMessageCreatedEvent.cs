namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinnedMessageCreatedEvent
{
    public required PinMessage Message { get; set; }
    
    /// <summary>
    ///     Duration how long the message is pinned.
    ///     Looks like it's just number of seconds (as string).
    /// </summary>
    /// <example>
    ///     1200
    /// </example>
    public required string Duration { get; set; }
    
    public required PinUser PinnedBy { get; set; }
}