namespace KickLib.Api.Unofficial.Models
{
    public class AuthenticationSettings
    {
        /// <summary>
        ///     Username for login.
        /// </summary>
        public string Username { get; }

        /// <summary>
        ///     Password for login.
        /// </summary>
        public string Password { get; }

        public bool UseOtp { get; set; } = true;

        /// <summary>
        ///     Two factor authentication code, which is display during initial 2FA setup.
        /// </summary>
        public string TwoFactorAuthCode { get; set; }

        /// <summary>
        ///     Static bearer token to use instead of logging in with username and password.
        ///     When this is set, Username and Password are ignored and authentication (login) is skipped.
        ///     Every call will use this token for authorization.
        /// </summary>
        /// <remarks>
        ///     await AuthenticateAsync() still needs to be called to apply the Bearer token to the client.
        /// </remarks>
        public string BearerTokenOverride { get; set; } = string.Empty;
        public bool HasTokenOverride => !string.IsNullOrWhiteSpace(BearerTokenOverride);

        public AuthenticationSettings(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
        
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            Username = username;
            Password = password;
        }

        private AuthenticationSettings()
        {
        }
        
        /// <summary>
        ///     Create authentication settings with only a bearer token override.
        /// </summary>
        public static AuthenticationSettings WithTokenOverride(string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new ArgumentNullException(nameof(bearerToken));
            }

            var sanitizedToken = bearerToken.Replace("Bearer", string.Empty, StringComparison.InvariantCultureIgnoreCase).Trim();
            var settings = new AuthenticationSettings
            {
                BearerTokenOverride = sanitizedToken
            };
            
            return settings;
        }
    }
}