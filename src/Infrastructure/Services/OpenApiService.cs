using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using WOF.Application.Common.Interfaces;
using OpenAI.GPT3.Interfaces;
using System.IO.Pipelines;
using System.IO;
using System.Net;

namespace WOF.Infrastructure.Services;

public class OpenApiService : IOpenApiService
{
    private readonly IOpenAIService _openAIService;

    public OpenApiService(IOpenAIService openAIService)
    {
        _openAIService = openAIService;
    }

    public async Task<string> GetChatResponse(string message, List<WOF.Application.Chat.Queries.GetResponse.ChatMessageVm> previousMessages)
    {
        var chatCompletionCreateRequest = CreateChatCompletionCreateRequest();
        if (previousMessages == null || previousMessages.Count == 0)
        {
            chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(message));
        }
        else
        {
            foreach (var previousChatMessage in previousMessages)
            {
                if (previousChatMessage.From == 1)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromAssistant(previousChatMessage.Message));

                }
                else if (previousChatMessage.From == 2)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(previousChatMessage.Message));
                }
                else if (previousChatMessage.From == 3)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromSystem(previousChatMessage.Message));
                }
            }
            chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(message));
        }
        var completionResult = await _openAIService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest);
        return completionResult.Choices.First().Message.Content;
    }

    public async Task<string> GetChatResponseFromSystem(string message, List<WOF.Application.Chat.Queries.GetResponse.ChatMessageVm> previousMessages)
    {
        var chatCompletionCreateRequest = CreateChatCompletionCreateRequest();
        if (previousMessages == null || previousMessages.Count == 0)
        {
            chatCompletionCreateRequest.Messages.Add(ChatMessage.FromSystem(message));
        }
        else
        {
            foreach (var previousChatMessage in previousMessages)
            {
                if (previousChatMessage.From == 1)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromAssistant(previousChatMessage.Message));

                }
                else if (previousChatMessage.From == 2)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromUser(previousChatMessage.Message));
                }
                else if (previousChatMessage.From == 3)
                {
                    chatCompletionCreateRequest.Messages.Add(ChatMessage.FromSystem(previousChatMessage.Message));
                }
            }
            chatCompletionCreateRequest.Messages.Add(ChatMessage.FromSystem(message));
        }
        var completionResult = await _openAIService.ChatCompletion.CreateCompletion(chatCompletionCreateRequest);
        return completionResult.Choices.First().Message.Content;
    }

    private ChatCompletionCreateRequest CreateChatCompletionCreateRequest()
    {
        var chatCompletionCreateRequest = new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a website assistant. You must respond only with JSON and no extra text."),
                ChatMessage.FromAssistant(
@"{
  ""cmd"": ""none"",
  ""response"": ""How may I help you manage your food item inventory?"",
}"
                    ),
                ChatMessage.FromUser("Change the bienenstich recipe to yummy"),
                ChatMessage.FromAssistant(
@"{
  ""cmd"": ""edit-recipe-name"",
  ""response"": ""Okay, I have updated your recipe."",
  ""original"": ""bienenstich"",
  ""new"": ""yummy""
}"
                    ),
                ChatMessage.FromUser("Can you substitute the bread for gluten free bread in the sandwich recipe?"),
                ChatMessage.FromAssistant(
@"{
  ""cmd"": ""edit-recipe-ingredient"",
  ""response"": ""Okay, I have substituted the bread ingredient for Gluten-free bread."",
  ""recipe"": ""sandwich"",
  ""original"": ""bread"",
  ""new"": ""Gluten-free bread""
}"
                    ),
                ChatMessage.FromUser("Order two cans of black eyed peas"),
                ChatMessage.FromAssistant(
@"{
  ""cmd"": ""order"",
  ""response"": ""Sure, I have added two cans of Black-eyed Peas to your cart."",
  ""items"": [
    {
      ""name"": ""Black-eyed Peas"",
      ""quantity"": 2
    }
  ]
}"
                    ),
                ChatMessage.FromUser("Create a new recipe that has chicken"),
                ChatMessage.FromAssistant(
@"{
  ""cmd"": ""create-recipe"",
  ""response"": ""Created."",
  ""name"": ""Chicken Pot Pie"",
  ""ingredients"": [
    {
      ""name"": ""chicken breast"",
      ""units"": ""2 cups cooked"",
    },
    {
      ""name"": ""mixed vegetables"",
      ""units"": ""1 (15 ounce) can"",
    },
    {
      ""name"": ""condensed cream of chicken soup"",
      ""units"": ""1 (10.5 ounce) can"",
    },
    {
      ""name"": ""milk"",
      ""units"": ""0.5 cups"",
    },
    {
      ""name"": ""deep-dish frozen pie crusts"",
      ""units"": ""2 (9 inch)"",
    },
  ]
}"),

            },
            Model = Models.ChatGpt3_5Turbo,
            //MaxTokens = 400,
            //FrequencyPenalty = -1,
            //PresencePenalty = -1,
            Temperature = 0.1f
        };
        return chatCompletionCreateRequest;
    }
}
