namespace KickLib.Api.Unofficial.Core
{
    public enum ApiVersion
    {
        V1 = 1,
        V2 = 2,
    
        V1Internal,
        // Some endpoints don't have /v1/ in the path
        None
    }
}