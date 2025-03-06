namespace KickLib.Tests;

public class BaseKickLibTests
{
    private readonly string _resourceFormat;
    
    public BaseKickLibTests(string basePath)
    {
        _resourceFormat = $"KickLib.Tests.{basePath}.{{0}}.json";
    }
    
    protected string GetPayload(string resourceName)
    {
        var assembly = typeof(BaseKickLibTests).Assembly;
        var filePath = string.Format(_resourceFormat, resourceName);
        using var stream = assembly.GetManifestResourceStream(filePath);
        if (stream is null)
        {
            throw new ArgumentException($"Missing resource file: {filePath}", nameof(resourceName));    
        }
        
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}