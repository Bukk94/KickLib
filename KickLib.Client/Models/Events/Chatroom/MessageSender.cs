namespace KickLib.Client.Models.Events.Chatroom;

public class MessageSender
{
    public int Id { get; set; }
    
    public string Username { get; set; }
    
    public string Slug { get; set; }

    public SenderIdentity Identity { get; set; }
}