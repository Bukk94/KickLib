namespace KickLib.Client.Models.Events.Livestream;

public class LivestreamWrapper<TType>
{
    public required TType Livestream { get; set; }
}