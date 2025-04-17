using System.Reflection;
using FluentAssertions;
using KickLib.Extensions;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Webhooks;

namespace KickLib.Tests;

public class WebhookTypesTests
{
    [Fact]
    public void WebhookTypes_HaveProperEnumValue()
    {
        var type = typeof(WebhookEventTypes);
        var values = type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(f => f.IsLiteral && !f.IsInitOnly)
            .Select(f => f.GetValue(null)!.ToString()!)
            .ToList();

        foreach (var value in values)
        {
            var ac = () => value.ToEventType();
            ac.Should().NotThrow();
            value.ToEventType().Should().NotBe(EventType.Unknown);
        }
    }

    [Fact]
    public void EventTypes_HaveProperNameRepresentation()
    {
        var enumValues = Enum.GetValues<EventType>().Except([EventType.Unknown]);

        foreach (var value in enumValues)
        {
            var ac = () => value.GetEventName();
            ac.Should().NotThrow();
        }
    }
}