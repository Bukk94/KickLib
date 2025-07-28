namespace KickLib.Models.v1.EventSubscriptions;

internal class SubscribeToEventRequest
{
    public IEnumerable<InputSubscribe> Events { get; }

    public string Method { get; set; } = "webhook";
    
    public SubscribeToEventRequest(IEnumerable<InputSubscribe> events)
    {
        Events = events;
    }
}