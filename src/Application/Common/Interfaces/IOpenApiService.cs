using WOF.Application.Chat.Queries.GetResponse;

namespace WOF.Application.Common.Interfaces;

public interface IOpenApiService
{
    Task<IChatResponse> GetChatResponse(string message, List<WOF.Application.Chat.Queries.GetResponse.ChatMessageVm> previousMessages);
}
