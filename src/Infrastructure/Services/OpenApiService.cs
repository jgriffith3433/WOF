using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using WOF.Application.Common.Interfaces;
using OpenAI.GPT3.Interfaces;
using WOF.Infrastructure.OpenApi.Responses;

namespace WOF.Infrastructure.Services;

public class OpenApiService : IOpenApiService
{
    private readonly IOpenAIService _openAIService;

    public OpenApiService(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<IChatResponse> GetChatResponse(string message, List<WOF.Application.Chat.Queries.GetResponse.ChatMessageVm> previousMessages)
    {
        var chatCompletionCreateRequest = new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant."),
                ChatMessage.FromUser("I am trying to manage my food item inventory."),
                ChatMessage.FromAssistant("How can I help you manage your food item inventory?"),
            },
            Model = Models.ChatGpt3_5Turbo,
            MaxTokens = 50//optional
        };
        if (previousMessages == null || previousMessages.Count == 0)
        {
            chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(message));
        }
        else
        {
            foreach(var previousChatMessage in previousMessages)
            {
                if (previousChatMessage.From == 1)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromAssistant(previousChatMessage.Message));

                }
                else if (previousChatMessage.From == 2)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(previousChatMessage.Message));
                }
            }
            chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(message));
        }
        var completionResult = await _openAIService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest);
        return new ChatResponse
        {
            Message = completionResult.Choices.First().Message.Content
        };
    }
}
