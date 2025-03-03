namespace KickLib.Exceptions
{
    public class KickLibException : Exception
    {
        public KickLibException(string message)
            : base(message)
        {
        }
    
        public KickLibException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}