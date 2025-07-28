using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace KickLib.Webhooks.Payloads;

/// <summary>
///     Details about user's badge.
/// </summary>
public class Badge
{
    /// <summary>
    ///     Text of the badge (e.g. 'moderator').
    /// </summary>
    public string Text { get; set; } = string.Empty;
    
    /// <summary>
    ///     Type of the badge.
    /// </summary>
    [JsonConverter(typeof(BadgeTypeConverter))]
    public BadgeType Type { get; set; }

    /// <summary>
    ///     Optional count value (e.g. number of months for a subscriber badge).
    /// </summary>
    public int? Count { get; set; }
    
    private class BadgeTypeConverter : StringEnumConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString();

            if (string.Equals(value, "sub_gifter", StringComparison.OrdinalIgnoreCase))
            {
                return BadgeType.SubGifter;
            }

            return Enum.TryParse(typeof(BadgeType), value, true, out var result)
                ? result
                : BadgeType.Unknown;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is BadgeType badgeType)
            {
                // Handle custom mappings for serialization
                if (badgeType == BadgeType.SubGifter)
                {
                    writer.WriteValue("sub_gifter");
                    return;
                }
            }

            base.WriteJson(writer, value, serializer);
        }
    }
}