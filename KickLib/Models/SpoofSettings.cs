namespace KickLib.Models;

public class SpoofSettings
{
    public string Ja3 { get; set; } = "";

    public string[] BackupJa3Fingerprints { get; set; } = [];
    
    /// <summary>
    ///     Creates empty SpoofSettings settings.
    /// </summary>
    public static SpoofSettings Empty => new();
}