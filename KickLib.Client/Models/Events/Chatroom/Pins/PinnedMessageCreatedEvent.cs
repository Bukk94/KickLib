namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinnedMessageCreatedEvent
{
    public PinMessage Message { get; set; } = new();
    
    /// <summary>
    ///     Duration how long the message is pinned.
    ///     Looks like it's just number of seconds (as string).
    /// </summary>
    /// <example>
    ///     1200
    /// </example>
    public string Duration { get; set; } = string.Empty;

    public PinUser PinnedBy { get; set; } = new();
}