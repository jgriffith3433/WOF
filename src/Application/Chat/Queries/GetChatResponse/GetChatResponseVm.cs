namespace WOF.Application.Chat.Queries.GetResponse;

public record GetChatResponseVm
{
    public List<ChatMessageVm> PreviousMessages { get; set; }
    public ChatMessageVm ResponseMessage { get; set; }
}
