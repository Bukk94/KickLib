namespace KickLib.Core;

/// <summary>
///     An event when a token in settings changes.
/// </summary>
public class TokenChangedEventArgs : EventArgs
{
    /// <summary>
    ///     Old token that was overwritten.
    /// </summary>
    public string? OldToken { get; }
    
    /// <summary>
    ///     New token, currently in use.
    /// </summary>
    public string? NewToken { get; }

    /// <summary>
    ///     A constructor for the TokenChangedEventArgs event.
    /// </summary>
    public TokenChangedEventArgs(string? oldToken, string? newToken)
    {
        OldToken = oldToken;
        NewToken = newToken;
    }
}