using FluentAssertions;
using KickLib.Core;
using KickLib.Models;
using KickLib.Models.v1.Auth;
using KickLib.Models.v1.Categories;
using KickLib.Models.v1.Channels;
using KickLib.Models.v1.Chat;
using KickLib.Models.v1.EventSubscriptions;
using KickLib.Models.v1.Users;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace KickLib.Tests;

/// <summary>
///     Tests for deserializing API responses.
/// </summary>
public class ApiResponseTests : BaseKickLibTests
{
    public ApiResponseTests() : base("Data.ApiResponses")
    {
    }
    
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        ContractResolver = new CamelCasePropertyNamesContractResolver(),
        Converters = new List<JsonConverter>
        {
            new StringEnumConverter(typeof(LowerCaseNamingStrategy))
        }
    };
    
    [Theory]
    [InlineData("GetCategoriesResponse", typeof(CategoryResponse))]
    public void CorrectlyDeserialize_CategoriesResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("GetPublicKeyResponse", typeof(PublicKeyResponse))]
    [InlineData("IntrospectTokenResponse", typeof(TokenIntrospectResponse))]
    [InlineData("IntrospectTokenResponse_Inactive", typeof(TokenIntrospectResponse))]
    public void CorrectlyDeserialize_AuthorizationResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("SendChatMessageResponse", typeof(SendChatMessageResponse))]
    public void CorrectlyDeserialize_ChatResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("SubscribeToEventResponse", typeof(SubscribeToEventResponse))]
    public void CorrectlyDeserialize_EventSubscriptionResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Theory]
    [InlineData("GetChannelsResponse", typeof(ChannelResponse))]
    public void CorrectlyDeserialize_ChannelsResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }

    [Theory]
    [InlineData("GetUsersResponse", typeof(UserResponse))]
    public void CorrectlyDeserialize_UsersResponses(string payloadResource, Type targetType)
    {
        var payload = GetPayload(payloadResource);
        
        var deserializedObject = JsonConvert.DeserializeObject(payload, targetType, SerializerSettings);
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType(targetType);
    }
    
    [Fact]
    public void CorrectlyDeserialize_WrapperObject()
    {
        var payload = GetPayload("WrapperResponse");
        
        var deserializedObject = JsonConvert.DeserializeObject<DataWrapper<TestClass>>(payload, SerializerSettings)!;
        
        deserializedObject.Should().NotBeNull();
        deserializedObject.Should().BeOfType<DataWrapper<TestClass>>();
    }

    private class TestClass
    {
        public required string Test { get; set; }
    }
}