namespace KickLib.Client.Models.Args;

public class UnknownEventArgs : EventArgs
{
    public string EventName { get; set; }
    public string RawData { get; set; }
    public EventSource Source { get; set; }
}