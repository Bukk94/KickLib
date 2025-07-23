namespace KickLib.Client.Models.Events.Chatroom.MessageReply;

public class OriginalMessageSender
{
    public required int Id { get; set; }

    // The username of the original message sender.
    public required string Username { get; set; }
}
