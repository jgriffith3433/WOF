namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandDeleteRecipe : OpenApiChatCommand
{
    public string Name { get; set; }
}
