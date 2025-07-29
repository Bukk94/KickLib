namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinUser : User
{
    public UserIdentity Identity { get; set; } = new();
}