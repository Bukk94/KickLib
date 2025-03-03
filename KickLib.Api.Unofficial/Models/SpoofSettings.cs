namespace KickLib.Api.Unofficial.Models
{
    public class SpoofSettings
    {
        public string Ja3 { get; set; } = "771,4865-4867-4866-49195-49199-52393-52392-49196-49200-49162-49161-49171-49172-51-57-47-53-10,0-23-65281-10-11-35-16-5-51-43-13-45-28-21,29-23-24-25-256-257,0";

        public string[] BackupJa3Fingerprints { get; set; } = [];
    
        /// <summary>
        ///     Creates empty SpoofSettings settings.
        /// </summary>
        public static SpoofSettings Empty => new();
    }
}