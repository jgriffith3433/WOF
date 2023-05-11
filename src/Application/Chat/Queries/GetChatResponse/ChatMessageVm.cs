namespace WOF.Application.Chat.Queries.GetResponse;

public record ChatMessageVm
{
    public int From { get; set; }
    public string Message { get; set; }
}
