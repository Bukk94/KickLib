namespace KickLib.Api.Unofficial.Models.Response.v2.Users;

public class UserIdentity
{
    public string Color { get; set; } = string.Empty;
    
    public ICollection<UserBadge> Badges { get; set; } = new List<UserBadge>();
}