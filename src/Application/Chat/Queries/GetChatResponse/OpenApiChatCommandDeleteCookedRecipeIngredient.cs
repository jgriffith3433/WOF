namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandDeleteCookedRecipeIngredient : OpenApiChatCommand
{
    public string Recipe { get; set; }
    public string Ingredient { get; set; }
}
