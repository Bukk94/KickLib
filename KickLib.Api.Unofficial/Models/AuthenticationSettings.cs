namespace KickLib.Api.Unofficial.Models
{
    public class AuthenticationSettings
    {
        public string Username { get; }

        public string Password { get; }

        public bool UseOtp { get; set; } = true;

        public string TwoFactorAuthCode { get; set; }

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
    }
}