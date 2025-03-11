namespace KickLib.Core;

public class TokenChangedEventArgs : EventArgs
{
    public string? NewToken { get; }

    public TokenChangedEventArgs(string? newToken)
    {
        NewToken = newToken;
    }
}