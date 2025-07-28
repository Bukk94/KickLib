namespace KickLib.Client.Models.Events.Livestream;

public class LivestreamEndedEvent
{
    public int Id { get; set; }

    public LivestreamChannel Channel { get; set; } = new();
}