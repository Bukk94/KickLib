namespace KickLib.Models.Response;

public class StatusResponse
{
    public bool Error { get; set; }
    public int Code { get; set; }
    public string Message { get; set; }
}