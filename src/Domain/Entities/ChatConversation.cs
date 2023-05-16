namespace WOF.Domain.Entities;

public class ChatConversation : BaseAuditableEntity
{
    public string Content { get; set; }
    public string? Error { get; set; }
}
