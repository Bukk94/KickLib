using JsonSubTypes;
using Newtonsoft.Json;

namespace KickLib.Api.Unofficial.Models.Response.v1.Emotes
{
    /// <summary>
    ///     Kick returns two types of emote response in one single array:
    ///     First one, containing 'name' is Kick's official list of emotes (emojis and global).
    ///     Second one, containing 'slug', is list of emotes of specific streamer.
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes))]
    [JsonSubtypes.KnownSubTypeWithProperty(typeof(KickEmotesResponse), "name")]
    [JsonSubtypes.KnownSubTypeWithProperty(typeof(UserEmotesResponse), "slug")]
    public abstract class EmotesResponse
    {
    }
}