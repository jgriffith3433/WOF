using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using WOF.Domain.Entities;
using Newtonsoft.Json;

namespace WOF.Application.Products.EventHandlers;

public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);
        var searchRequest = new SearchRequest
        {
            query = notification.Product.Name
        };

        var searchResponse = searchRequest.GetResponse<SearchResponse>().Result;
        notification.Product.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        return _context.SaveChangesAsync(cancellationToken);
    }
}
