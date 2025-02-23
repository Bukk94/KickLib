namespace KickLib.Client.Models.Args;

public class UnknownEventArgs : EventArgs
{
    public required string EventName { get; set; }
    
    public required string RawData { get; set; }
    
    public EventSource Source { get; set; }
}