namespace KickLib.Models.Response;

public class DataWrapper<TType>
{
    public TType Data { get; set; }
    public StatusResponse Status { get; set; }
}