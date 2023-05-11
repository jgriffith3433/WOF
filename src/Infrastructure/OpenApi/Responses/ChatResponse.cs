using WOF.Application.Common.Interfaces;

namespace WOF.Infrastructure.OpenApi.Responses;


public class ChatResponse : IChatResponse
{
    public string Message { get; set; }
}
