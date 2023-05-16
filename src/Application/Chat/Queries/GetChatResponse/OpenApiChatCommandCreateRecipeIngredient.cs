namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandCreateRecipeIngredient : OpenApiChatCommand
{
    public string Name { get; set; }
    public string Units { get; set; }
}
