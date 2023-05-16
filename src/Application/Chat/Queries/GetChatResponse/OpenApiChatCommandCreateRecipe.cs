namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandCreateRecipe : OpenApiChatCommand
{
    public string Name { get; set; }
    public List<OpenApiChatCommandCreateRecipeIngredient> Ingredients { get; set; }
}
