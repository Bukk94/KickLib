namespace KickLib.Client.Models.Events.Chatroom;

public class User
{
    public int Id { get; set; }

    public required string Username { get; set; }

    public required string Slug { get; set; }
}