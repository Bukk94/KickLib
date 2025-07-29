namespace KickLib.Client.Models.Args;

public class UnknownEventArgs : EventArgs
{
    public string EventName { get; set; } = string.Empty;
    
    public string RawData { get; set; } = string.Empty;
    
    public EventSource Source { get; set; }
}