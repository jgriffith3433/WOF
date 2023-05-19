namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandGoToPage : OpenApiChatCommand
{
    public string Page { get; set; }
}
