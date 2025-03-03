namespace KickLib.Api.Unofficial.Models.Response.v2.Channels.Messages
{
    public class MessageSenderResponse
    {
        public int Id { get; set; }

        public string Slug { get; set; }

        public string Username { get; set; }

        public SenderIdentityResponse Identity { get; set; }
    }
}