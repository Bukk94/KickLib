using System.Net;

namespace KickLib.Api.Unofficial.Clients.CycleTls
{
    public class CycleTlsRequestOptions
    {
        public string Url { get; set; } = null;
        public string Method { get; set; } = null;
        public Dictionary<string, string> Headers { get; set; } = null;
        public string Body { get; set; } = null;
        public string Ja3 { get; set; } = null;
        public string UserAgent { get; set; } = null;
        public string Proxy { get; set; } = null;
        public List<Cookie> Cookies { get; set; } = null;
        public int? Timeout { get; set; } = null;
        public bool? DisableRedirect { get; set; } = null;
        public List<string> HeaderOrder { get; set; } = null;
        public bool? OrderAsProvided { get; set; } = null;
    }
}