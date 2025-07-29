namespace KickLib.Client.Models.Events.Livestream;

public class LivestreamWrapper<TType>
{
    public TType Livestream { get; set; } = default!;
}