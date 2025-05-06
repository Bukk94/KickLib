namespace KickLib.Models
{
    /// <summary>
    ///     DTO data wrapper for all Kick API responses.
    /// </summary>
    /// <typeparam name="TType"></typeparam>
    public class DataWrapper<TType>
    {
        /// <summary>
        ///     Actual response data.
        /// </summary>
        public TType? Data { get; set; }
        
        /// <summary>
        ///     Optional response message.
        /// </summary>
        public string? Message { get; set; }
    }
}