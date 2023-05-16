namespace WOF.Domain.Entities;

public class ChatCommand : BaseAuditableEntity
{
    public string CommandName { get; set; }
    public string? SystemResponse { get; set; }
    public string RawReponse { get; set; }
    public bool Unknown { get; set; }
    public bool ChangedData { get; set; }
    public ChatConversation ChatConversation { get; set; }
}
