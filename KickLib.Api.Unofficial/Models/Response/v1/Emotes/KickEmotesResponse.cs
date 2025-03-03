namespace KickLib.Api.Unofficial.Models.Response.v1.Emotes
{
    public class KickEmotesResponse : EmotesResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<EmoteResponse> Emotes { get; set; }
    }
}