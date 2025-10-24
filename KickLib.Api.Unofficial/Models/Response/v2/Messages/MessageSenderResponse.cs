using KickLib.Api.Unofficial.Models.Response.v2.Users;

namespace KickLib.Api.Unofficial.Models.Response.v2.Messages;

public class MessageSenderResponse
{
    public int Id { get; set; }
    
    public string Username { get; set; } = string.Empty;
    
    public string Slug { get; set; } = string.Empty;

    public UserIdentity Identity { get; set; }
}