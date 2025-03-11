namespace KickLib.Core;

public class ApiSettings
{
    public event EventHandler<TokenChangedEventArgs>? AccessTokenChanged;
    public event EventHandler<TokenChangedEventArgs>? RefreshTokenChanged;

    private string? _accessToken;
    private string? _refreshToken;

    public string? AccessToken
    {
        get => _accessToken;
        set
        {
            if (_accessToken != value)
            {
                _accessToken = value;
                AccessTokenChanged?.Invoke(this, new TokenChangedEventArgs(_accessToken));
            }
        }
    }

    public string? RefreshToken
    {
        get => _refreshToken;
        set
        {
            if (_refreshToken != value)
            {
                _refreshToken = value;
                RefreshTokenChanged?.Invoke(this, new TokenChangedEventArgs(_refreshToken));
            }
        }
    }

    public string? ClientId { get; set; }

    public string? ClientSecret { get; set; }

    /// <summary>
    ///     When set to true, API will throw an exception when deserialization fails.
    ///     Default: false
    ///     Enabling this might cause instability as Kick API is still subject to change without further notice.
    /// </summary>
    public bool ThrowOnDeserializationError { get; set; }

    public bool CanRefreshToken =>
        !string.IsNullOrWhiteSpace(RefreshToken) &&
        !string.IsNullOrWhiteSpace(ClientId) &&
        !string.IsNullOrWhiteSpace(ClientSecret);
}