using KickLib.Models;

namespace KickLib.Core;

internal class ResponseMetadata : ISuccess
{
    public string Message { get; } = string.Empty;
    public Dictionary<string, object> Metadata { get; }

    public ResponseMetadata(Pagination pagination)
    {
        Metadata = new Dictionary<string, object>
        {
            { $"{nameof(Pagination)}", pagination }
        };
    }
        
    public Pagination GetPagination()
    {
        return (Pagination)Metadata[$"{nameof(Pagination)}"];
    }
}