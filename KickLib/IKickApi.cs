using KickLib.Api;

namespace KickLib;

/// <summary>
///     The Kick API interface. This interface provides access to all the APIs available in the KickLib.
/// </summary>
public interface IKickApi
{
    /// <summary>
    ///     Authorization-related APIs to allow you to interact with OAuth endpoints.
    /// </summary>
    public Authorization Authorization { get; }

    /// <summary>
    ///     Categories APIs allow you to use and interact with the categories that are available on the Kick website.
    /// </summary>
    public Categories Categories { get; }
    
    /// <summary>
    ///     Chat APIs allow you to use and interact with the chat that is available on the Kick website. You can send a message as a Bot account or your User account.
    /// </summary>
    public Chat Chat { get; }
    
    /// <summary>
    ///     Channels APIs allow an app to interact with channels in the Kick website. Available data will depend on the scopes attached to the authorization token used.
    /// </summary>
    public Channels Channels { get; }
    
    /// <summary>
    ///     User APIs allow apps to interact with user information. Scopes will vary and sensitive data will be available to User Access Tokens with the required scopes.
    /// </summary>
    public Users Users { get; }
}