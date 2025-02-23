namespace KickLib.Client.Models.Events.Chatroom;

public class MessageSender
{
    public int Id { get; set; }
    
    public required string Username { get; set; }
    
    public required string Slug { get; set; }

    public required SenderIdentity Identity { get; set; }
}