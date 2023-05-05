using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace WOF.Application.CompletedOrders.EventHandlers;

public class CompletedOrderProductCreatedEventHandler : INotificationHandler<CompletedOrderProductCreatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CompletedOrderProductCreatedEventHandler> _logger;

    public CompletedOrderProductCreatedEventHandler(ILogger<CompletedOrderProductCreatedEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(CompletedOrderProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);
        var searchRequest = new SearchRequest
        {
            query = notification.CompletedOrderProduct.Name
        };

        var searchResponse = searchRequest.GetResponse<SearchResponse>().Result;
        notification.CompletedOrderProduct.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        return _context.SaveChangesAsync(cancellationToken);
    }
}
