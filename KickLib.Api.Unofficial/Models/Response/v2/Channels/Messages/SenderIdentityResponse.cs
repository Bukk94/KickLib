namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Messages
{
    public class SenderIdentityResponse
    {
        /// <summary>
        ///     Hex color value.
        /// </summary>
        public string Color { get; set; }

        public ICollection<SenderBadgeResponse> Badges { get; set; }
    }
}