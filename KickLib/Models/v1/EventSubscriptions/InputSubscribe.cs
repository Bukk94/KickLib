using KickLib.Extensions;

namespace KickLib.Models.v1.EventSubscriptions;

internal class InputSubscribe
{
    public InputSubscribe(EventType type, int version)
    {
        Name = type.GetEventName();
        Version = version;
    }

    public string Name { get; }

    public int Version { get; }
}