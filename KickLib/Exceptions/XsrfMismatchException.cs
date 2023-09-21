namespace KickLib.Exceptions;

public class XsrfMismatchException : Exception
{
    public XsrfMismatchException(string msg) : base(msg)
    {
    }
}