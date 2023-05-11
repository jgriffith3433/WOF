using AutoMapper;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;

namespace WOF.Application.Chat.Queries.GetResponse;

[Authorize]
public record GetChatResponseQuery : IRequest<GetChatResponseVm>
{
    public List<ChatMessageVm> PreviousMessages { get; set; }
    public ChatMessageVm ChatMessage { get; set; }
}

public class GetChatResponseQueryHandler : IRequestHandler<GetChatResponseQuery, GetChatResponseVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IOpenApiService _openApiService;

    public GetChatResponseQueryHandler(IApplicationDbContext context, IMapper mapper, IOpenApiService openApiService)
    {
        _context = context;
        _mapper = mapper;
        _openApiService = openApiService;
    }

    public async Task<GetChatResponseVm> Handle(GetChatResponseQuery request, CancellationToken cancellationToken)
    {
        var chatResponse = await _openApiService.GetChatResponse(request.ChatMessage.Message, request.PreviousMessages);
        if (request.PreviousMessages == null)
        {
            request.PreviousMessages = new List<ChatMessageVm>();
        }
        request.PreviousMessages.Add(request.ChatMessage);
        request.PreviousMessages.Add(new ChatMessageVm
        {
            From = 1,
            Message = chatResponse.Message
        });
        return new GetChatResponseVm
        {
            ResponseMessage = new ChatMessageVm
            {
                From = 1,
                Message = chatResponse.Message
            },
            PreviousMessages = request.PreviousMessages
        };
    }
}
