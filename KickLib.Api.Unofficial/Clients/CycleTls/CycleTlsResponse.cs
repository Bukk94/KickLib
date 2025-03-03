namespace KickLib.Api.Unofficial.Clients.CycleTls
{
    public class CycleTlsResponse
    {
        public string RequestID { get; set; }
        public int Status { get; set; }
        public string Body { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }
}