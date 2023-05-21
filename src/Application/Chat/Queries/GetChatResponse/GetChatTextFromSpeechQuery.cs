using AutoMapper;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;

namespace WOF.Application.Chat.Queries.GetResponse;

[Authorize]
public record GetChatTextFromSpeechQuery : IRequest<GetChatTextFromSpeechVm>
{
    public byte[] Speech { get; set; }
}

public class GetChatTextFromSpeechQueryHandler : IRequestHandler<GetChatTextFromSpeechQuery, GetChatTextFromSpeechVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IOpenApiService _openApiService;

    public GetChatTextFromSpeechQueryHandler(IApplicationDbContext context, IMapper mapper, IOpenApiService openApiService)
    {
        _context = context;
        _mapper = mapper;
        _openApiService = openApiService;
    }

    public async Task<GetChatTextFromSpeechVm> Handle(GetChatTextFromSpeechQuery request, CancellationToken cancellationToken)
    {
        var speechToTextMessage = await _openApiService.GetTextFromSpeech(request.Speech);

        return new GetChatTextFromSpeechVm
        {
            Text = speechToTextMessage
        };
    }
}
