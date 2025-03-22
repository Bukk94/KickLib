namespace KickLib.Core;

public class TokenChangedEventArgs : EventArgs
{
    public string? OldToken { get; }
    
    public string? NewToken { get; }

    public TokenChangedEventArgs(string? oldToken, string? newToken)
    {
        OldToken = oldToken;
        NewToken = newToken;
    }
}