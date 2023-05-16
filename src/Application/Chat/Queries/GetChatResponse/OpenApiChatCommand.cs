namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommand
{
    public string Cmd { get; set; }
    public string Response { get; set; }
}
