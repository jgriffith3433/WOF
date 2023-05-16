namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandEditRecipeName : OpenApiChatCommand
{
    public string Original { get; set; }
    public string New { get; set; }
}
