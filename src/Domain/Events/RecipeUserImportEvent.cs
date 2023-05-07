namespace WOF.Domain.Events;

public class RecipeUserImportEvent : BaseEvent
{
    public RecipeUserImportEvent(Recipe recipe)
    {
        Recipe = recipe;
    }

    public Recipe Recipe { get; }
}
