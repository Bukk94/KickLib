namespace KickLib.Client.Models.Events.Chatroom.MessageReply;

public class OriginalMessageSender
{
    public required int Id { get; set; }

    /// <summary>
    ///     The username of the original message sender.
    /// </summary>
    public required string Username { get; set; }
}
