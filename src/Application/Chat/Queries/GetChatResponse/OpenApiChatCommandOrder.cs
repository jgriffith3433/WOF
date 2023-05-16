namespace WOF.Application.Chat.Queries.GetResponse;

public record OpenApiChatCommandOrder : OpenApiChatCommand
{
    public List<OpenApiChatCommandOrderItem> Items { get; set; }
}
