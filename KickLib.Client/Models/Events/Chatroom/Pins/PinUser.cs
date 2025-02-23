namespace KickLib.Client.Models.Events.Chatroom.Pins;

public class PinUser : User
{
    public required UserIdentity Identity { get; set; }
}