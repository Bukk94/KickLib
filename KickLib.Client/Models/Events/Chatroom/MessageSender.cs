namespace KickLib.Client.Models.Events.Chatroom;

public class MessageSender
{
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Slug { get; set; } = string.Empty;

    public SenderIdentity Identity { get; set; } = new();
}