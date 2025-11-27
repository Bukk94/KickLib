namespace KickLib.Core;

/// <summary>
///     Settings for KickLib API calls to Kick.com.
/// </summary>
public class ApiSettings
{
    /// <summary>
    ///    Default API settings.
    /// </summary>
    public static ApiSettings Default => new();
    
    /// <summary>
    ///     Fires when access token changes.
    /// </summary>
    public event EventHandler<TokenChangedEventArgs>? AccessTokenChanged;
    
    /// <summary>
    ///     Fires when refresh token changes.
    /// </summary>
    public event EventHandler<TokenChangedEventArgs>? RefreshTokenChanged;

    private string? _accessToken;
    private string? _refreshToken;

    /// <summary>
    ///     The access token (Bearer) used for API requests.
    /// </summary>
    public string? AccessToken
    {
        get => _accessToken;
        set
        {
            if (string.Equals(_accessToken, value))
            {
                return;
            }
            
            var arg = new TokenChangedEventArgs(_accessToken, value);
            _accessToken = value;
            AccessTokenChanged?.Invoke(this, arg);
        }
    }

    /// <summary>
    ///     A refresh token (long-lived token) used to obtain a new access tokens on behalf of the user.
    /// </summary>
    public string? RefreshToken
    {
        get => _refreshToken;
        set
        {
            if (string.Equals(_refreshToken, value))
            {
                return;
            }

            var arg = new TokenChangedEventArgs(_refreshToken, value);
            _refreshToken = value;
            RefreshTokenChanged?.Invoke(this, arg);
        }
    }

    /// <summary>
    ///     Application Client ID.
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    ///     Application Client Secret.
    /// </summary>
    public string? ClientSecret { get; set; }

    /// <summary>
    ///     When set to true, API will throw an exception when deserialization fails.
    ///     Default: false
    ///     Enabling this might cause instability as Kick API is still subject to change without further notice.
    /// </summary>
    public bool ThrowOnDeserializationError { get; set; }

    internal bool CanRefreshToken =>
        !string.IsNullOrWhiteSpace(RefreshToken) &&
        !string.IsNullOrWhiteSpace(ClientId) &&
        !string.IsNullOrWhiteSpace(ClientSecret);
}