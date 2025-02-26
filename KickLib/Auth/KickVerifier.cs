﻿namespace KickLib.Auth
{
    /// <summary>
    ///     Kick verifier format.
    /// </summary>
    public class KickVerifier
    {
        /// <summary>
        ///     Code verifier.
        /// </summary>
        public required string CodeVerifier { get; set; }
    }
}