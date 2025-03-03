namespace KickLib.Models
{
    public class DataWrapper<TType>
    {
        public TType? Data { get; set; }
        
        public string? Message { get; set; }
    }
}