namespace KickLib.Client.Models.Events.Chatroom;

public class MessageDeletedEvent
{
    public Guid Id { get; set; }

    public IdEnvelope Message { get; set; } = new();
    
    public bool AiModerated { get; set; }

    // public ICollection<ViolatedRule> ViolatedRules { get; set; }
}