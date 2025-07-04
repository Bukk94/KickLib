# Optimized KickLib Usage Examples

This file demonstrates how to use the optimized KickLib with multi-user, shared-browser support.

## Basic Usage

### Single User

```csharp
using KickLib.Api.Unofficial;
using KickLib.Api.Unofficial.Models;

// Create optimized API instance
var kickApi = new OptimizedKickUnofficialApi();

// User authentication
var authSettings = new AuthenticationSettings
{
    Username = "user@email.com",
    Password = "password123",
    UseOtp = true,
    TwoFactorAuthCode = "2FA_SECRET_KEY"
};

await kickApi.AuthenticateAsync(authSettings);

// API usage
var channels = await kickApi.Channels.GetChannelsAsync();
```

### Multiple Users

```csharp
using KickLib.Api.Unofficial;
using KickLib.Api.Unofficial.Models;

// API instance for User A
var userAApi = OptimizedKickUnofficialApi.CreateForUser("userA");
var authSettingsA = new AuthenticationSettings
{
    Username = "userA@email.com",
    Password = "passwordA"
};
await userAApi.AuthenticateAsync(authSettingsA);

// API instance for User B (uses same browser instance)
var userBApi = OptimizedKickUnofficialApi.CreateForUser("userB");
var authSettingsB = new AuthenticationSettings
{
    Username = "userB@email.com",
    Password = "passwordB"
};
await userBApi.AuthenticateAsync(authSettingsB);

// Each user can operate independently with their own session
var userAChannels = await userAApi.Channels.GetChannelsAsync();
var userBChannels = await userBApi.Channels.GetChannelsAsync();
```

## Dependency Injection Usage

### Startup.cs / Program.cs

```csharp
using KickLib.Api.Unofficial.Extensions;
using KickLib.Api.Unofficial.Models;

var builder = WebApplication.CreateBuilder(args);

// Add optimized KickLib
var browserSettings = new BrowserSettings
{
    EnableBrowserFetching = true,
    LaunchOptions = new LaunchOptions
    {
        Headless = true,
        Args = new[] { "--no-sandbox", "--disable-setuid-sandbox" }
    }
};

builder.Services.AddOptimizedUnofficialKickLib()
    .WithOptimizedPuppeteerClient(browserSettings);

var app = builder.Build();
```

### Controller Usage

```csharp
[ApiController]
[Route("api/[controller]")]
public class KickController : ControllerBase
{
    private readonly IOptimizedKickUnofficialApiFactory _apiFactory;
    private OptimizedKickUnofficialApi _kickApi;

    public KickController(IOptimizedKickUnofficialApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
        // Separate session for each user
        var userId = HttpContext.User.Identity.Name ?? Guid.NewGuid().ToString();
        _kickApi = _apiFactory.CreateInstance(userId);
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticationSettings authSettings)
    {
        try
        {
            await _kickApi.AuthenticateAsync(authSettings);
            return Ok(new { success = true, sessionId = _kickApi.SessionId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet("channels")]
    public async Task<IActionResult> GetChannels()
    {
        if (!_kickApi.IsAuthenticated)
        {
            return Unauthorized("Please authenticate first");
        }

        var channels = await _kickApi.Channels.GetChannelsAsync();
        return Ok(channels);
    }
}
```

## Session Management

### View Active Sessions

```csharp
// Get all active sessions
var activeSessions = OptimizedKickUnofficialApi.GetActiveSessions();
Console.WriteLine($"Active sessions: {string.Join(", ", activeSessions)}");
```

### Cleanup Expired Sessions

```csharp
// Clean up sessions older than 30 minutes
OptimizedKickUnofficialApi.CleanupExpiredSessions(TimeSpan.FromMinutes(30));
```

### Remove Specific Session

```csharp
// Remove a specific session
OptimizedKickUnofficialApi.RemoveSession("userA");
```

## Performance Optimizations

### Browser Settings

```csharp
var optimizedBrowserSettings = new BrowserSettings
{
    EnableBrowserFetching = true,
    LaunchOptions = new LaunchOptions
    {
        Headless = true,
        Args = new[]
        {
            "--no-sandbox",
            "--disable-setuid-sandbox",
            "--disable-dev-shm-usage",
            "--disable-gpu",
            "--disable-background-timer-throttling",
            "--disable-backgrounding-occluded-windows",
            "--disable-renderer-backgrounding",
            "--memory-pressure-off"
        }
    }
};

var kickApi = new OptimizedKickUnofficialApi(browserSettings: optimizedBrowserSettings);
```

### Get Session Info

```csharp
var sessionInfo = kickApi.GetSessionInfo();
Console.WriteLine($"Session ID: {sessionInfo.SessionId}");
Console.WriteLine($"Authenticated: {sessionInfo.IsAuthenticated}");
Console.WriteLine($"Created: {sessionInfo.CreatedAt}");
Console.WriteLine($"Last Accessed: {sessionInfo.LastAccessedAt}");
```

## Important Notes

1. **Shared Browser Instance**: All sessions share the same browser instance, optimizing memory usage.

2. **Automatic Resource Blocking**: Unnecessary resources like images, CSS, fonts are automatically blocked for faster requests.

3. **Session Isolation**: Each user has their own authentication credentials and doesn't affect others.

4. **Error Handling**: Issues like CSRF token mismatches are automatically handled with retry logic.

5. **Memory Management**: Unused sessions can be automatically cleaned up.

6. **Concurrent Requests**: Multiple users can use the API simultaneously.

## Factory Pattern Usage (Recommended)

```csharp
using KickLib.Api.Unofficial.Interfaces;

// Inject the factory
public class MyService
{
    private readonly IOptimizedKickUnofficialApiFactory _apiFactory;

    public MyService(IOptimizedKickUnofficialApiFactory apiFactory)
    {
        _apiFactory = apiFactory;
    }

    public async Task<string> ProcessUserData(string userId)
    {
        // Create API instance for specific user
        var userApi = _apiFactory.CreateInstance(userId);
        
        // Use the API
        var channels = await userApi.Channels.GetChannelsAsync();
        
        return $"User {userId} has {channels.Count()} channels";
    }
}
```
