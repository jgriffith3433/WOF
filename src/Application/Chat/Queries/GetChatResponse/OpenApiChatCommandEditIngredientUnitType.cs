namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandEditIngredientUnitType : OpenApiChatCommand
{
    public string Name { get; set; }
    public string Ingredient
    {
        get { return Name; }
        set { Name = value; }
    }
    public string UnitType { get; set; }
    public string New
    {
        get { return UnitType; }
        set { UnitType = value; }
    }
    public string New_UnitType
    {
        get { return UnitType; }
        set { UnitType = value; }
    }
}
