namespace KickLib.Client.Models.Events.Chatroom.MessageReply;

public class OriginalMessageSender
{
    public int Id { get; set; }

    /// <summary>
    ///     The username of the original message sender.
    /// </summary>
    public string Username { get; set; } = string.Empty;
}
