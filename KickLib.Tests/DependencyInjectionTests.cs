using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace KickLib.Tests;

public class DependencyInjectionTests
{
    [Fact]
    public void Injection_DoesNotFail()
    {
        var serviceProvider = CreateProvider();
        
        var act = () => serviceProvider.GetRequiredService<IKickApi>();
        act.Should().NotThrow();
    }
    
    [Fact]
    public void KickApi_IsInjectedProperly()
    {
        var serviceProvider = CreateProvider();
        
        var kickApi = serviceProvider.GetRequiredService<IKickApi>();
        kickApi.Should().NotBeNull();
    }

    [Fact]
    public void KickApi_CreateFactory_DoesNotThrow()
    {
        var kickApi = () => KickApi.Create();
        kickApi.Should().NotThrow();
    }
    
    [Fact]
    public void KickApi_CreateFactory_ContainsAllValues()
    {
        var kickApi = KickApi.Create();
        kickApi.Should().NotBeNull();
        kickApi.ApiSettings.Should().NotBeNull();
        kickApi.Authorization.Should().NotBeNull();
        kickApi.Categories.Should().NotBeNull();
        kickApi.Channels.Should().NotBeNull();
        kickApi.Chat.Should().NotBeNull();
        kickApi.EventSubscriptions.Should().NotBeNull();
        kickApi.Livestreams.Should().NotBeNull();
        kickApi.Users.Should().NotBeNull();
    }
    
    private static IServiceProvider CreateProvider()
    {
        return new ServiceCollection()
            .AddKickLib()
            .BuildServiceProvider();
    }
}