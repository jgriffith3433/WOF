namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandEditProductUnitType : OpenApiChatCommand
{
    public string Product { get; set; }

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
