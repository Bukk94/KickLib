namespace KickLib.Interfaces;

public interface IApiCaller
{
    Task<KeyValuePair<int, string>> SendRequestAsync(string url);
}
