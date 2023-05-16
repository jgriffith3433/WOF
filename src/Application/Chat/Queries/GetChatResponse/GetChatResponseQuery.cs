using AutoMapper;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Newtonsoft.Json;
using WOF.Domain.Entities;
using WOF.Application.Common.Exceptions;
using WOF.Domain.Events;

namespace WOF.Application.Chat.Queries.GetResponse;

[Authorize]
public record GetChatResponseQuery : IRequest<GetChatResponseVm>
{
    public List<ChatMessageVm> PreviousMessages { get; set; } = new List<ChatMessageVm> { };
    public ChatMessageVm ChatMessage { get; set; }
    public int? ChatConversationId { get; set; }
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
        var chatResponseMessage = await _openApiService.GetChatResponse(request.ChatMessage.Message, request.PreviousMessages);

        request.PreviousMessages.Add(request.ChatMessage);
        request.PreviousMessages.Add(new ChatMessageVm
        {
            From = 1,
            Message = chatResponseMessage
        });
        ChatConversation chatConversationEntity;
        if (request.ChatConversationId.HasValue)
        {
            chatConversationEntity = _context.ChatConversations.FirstOrDefault(cc => cc.Id == request.ChatConversationId.Value);
            if (chatConversationEntity == null)
            {
                throw new NotFoundException(nameof(ChatConversation), request.ChatConversationId);
            }
            chatConversationEntity.Content = JsonConvert.SerializeObject(request);
        }
        else
        {
            chatConversationEntity = new ChatConversation
            {
                Content = JsonConvert.SerializeObject(request),
            };
            _context.ChatConversations.Add(chatConversationEntity);
        }

        await _context.SaveChangesAsync(cancellationToken);
        var dirty = false;
        try
        {
            var chatCommandEntity = new ChatCommand
            {
                RawReponse = chatResponseMessage
            };
            var startIndex = chatResponseMessage.IndexOf('{');
            var endIndex = chatResponseMessage.LastIndexOf('}');
            chatResponseMessage = chatResponseMessage.Substring(startIndex, endIndex - startIndex + 1);
            var openApiChatCommand = JsonConvert.DeserializeObject<OpenApiChatCommand>(chatResponseMessage);

            chatCommandEntity.CommandName = openApiChatCommand.Cmd;
            chatCommandEntity.ChatConversation = chatConversationEntity;
            _context.ChatCommands.Add(chatCommandEntity);

            //TODO: Do we need to call SaveChangesAsync before the domain event is sent so the change tracker HasChanges only if the event modified entities?
            //calling for now
            await _context.SaveChangesAsync(cancellationToken);
            chatCommandEntity.AddDomainEvent(new ReceivedChatCommandEvent(chatCommandEntity));
            await _context.SaveChangesAsync(cancellationToken);
            dirty = chatCommandEntity.ChangedData;

            if (!string.IsNullOrEmpty(chatCommandEntity.SystemResponse))
            {
                var chatSystemResponseMessage = await _openApiService.GetChatResponseFromSystem(chatCommandEntity.SystemResponse, request.PreviousMessages);
                startIndex = chatSystemResponseMessage.IndexOf('{');
                endIndex = chatSystemResponseMessage.LastIndexOf('}');
                if (startIndex == -1) { startIndex = 0; }
                if (endIndex == -1) { endIndex = chatSystemResponseMessage.Length - 1; }
                chatSystemResponseMessage = chatSystemResponseMessage.Substring(startIndex, endIndex - startIndex + 1);

                request.PreviousMessages.Add(new ChatMessageVm
                {
                    From = 3,
                    Message = chatCommandEntity.SystemResponse
                });
                request.PreviousMessages.Add(new ChatMessageVm
                {
                    From = 1,
                    Message = chatSystemResponseMessage
                });

                chatConversationEntity.Content = JsonConvert.SerializeObject(request);
                chatResponseMessage = chatSystemResponseMessage;
            }
            else
            {
                chatResponseMessage = openApiChatCommand.Response;
            }
        }
        catch (Exception e)
        { 
            Console.WriteLine(e.ToString() + e.Message.ToString());
        }

        return new GetChatResponseVm
        {
            ChatConversationId = chatConversationEntity.Id,
            Dirty = dirty,
            ResponseMessage = new ChatMessageVm
            {
                From = 1,
                Message = chatResponseMessage
            },
            PreviousMessages = request.PreviousMessages
        };
    }
}
