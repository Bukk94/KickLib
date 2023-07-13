namespace KickLib.Core;

public enum ApiVersion
{
    V1 = 1,
    V2 = 2,
    
    // Some endpoints don't have /v1/ in the path
    None
}