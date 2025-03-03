namespace KickLib.Api.Unofficial.Exceptions
{
    public class KickUnavailableException : Exception
    {
        public KickUnavailableException()
            : base("Kick.com website occurred error and returned Server Error 500.")
        {
        }
    }
}