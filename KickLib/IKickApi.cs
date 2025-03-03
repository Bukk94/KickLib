using KickLib.Api;

namespace KickLib;

public interface IKickApi
{
    /// <summary>
    ///     Categories APIs allow you to use and interact with the categories that are available on the Kick website.
    /// </summary>
    public Categories Categories { get; }
    
    /// <summary>
    ///     Channels APIs allow an app to interact with channels in the Kick website. Available data will depend on the scopes attached to the authorization token used.
    /// </summary>
    public Channels Channels { get; }
    
    /// <summary>
    ///     User APIs allow apps to interact with user information. Scopes will vary and sensitive data will be available to User Access Tokens with the required scopes.
    /// </summary>
    public Users Users { get; }
}