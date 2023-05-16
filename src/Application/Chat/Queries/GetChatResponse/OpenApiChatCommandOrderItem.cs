namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandOrderItem : OpenApiChatCommand
{
    public string Name { get; set; }
    public long Quantity { get; set; }
}
