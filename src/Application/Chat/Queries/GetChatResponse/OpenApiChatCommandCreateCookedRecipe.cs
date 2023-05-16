namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandCreateCookedRecipe : OpenApiChatCommand
{
    public string Name { get; set; }
    public string Recipe
    {
        get { return Name; }
        set { Name = value; }
    }
}
