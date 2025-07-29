namespace KickLib.Auth
{
    /// <summary>
    ///     Kick verifier format.
    /// </summary>
    public class KickVerifier
    {
        /// <summary>
        ///     Code verifier.
        /// </summary>
        public string CodeVerifier { get; set; } = string.Empty;
    }
}