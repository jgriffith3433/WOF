namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandCreateRecipeIngredient : OpenApiChatCommand
{
    public string Name { get; set; }
    public float? Units { get; set; }
    public string? UnitType { get; set; }
}
