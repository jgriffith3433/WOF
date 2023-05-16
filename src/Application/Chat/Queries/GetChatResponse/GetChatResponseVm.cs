namespace WOF.Application.Chat.Queries.GetResponse;

public record GetChatResponseVm
{
    public int ChatConversationId { get; set; }
    public bool Dirty { get; set; }
    public List<ChatMessageVm> PreviousMessages { get; set; }
    public ChatMessageVm ResponseMessage { get; set; }
}
