namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandDeleteRecipeIngredient : OpenApiChatCommand
{
    public string Recipe { get; set; }
    public string Ingredient { get; set; }
}
