
namespace WOF.Application.Common.Interfaces;

public interface IOpenApiService
{
    Task<string> GetChatResponse(string message, List<WOF.Application.Chat.Queries.GetResponse.ChatMessageVm> previousMessages);
    Task<string> GetChatResponseFromSystem(string message, List<WOF.Application.Chat.Queries.GetResponse.ChatMessageVm> previousMessages);
}
