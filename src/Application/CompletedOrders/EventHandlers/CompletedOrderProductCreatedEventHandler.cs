﻿using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Common.Interfaces;
using Newtonsoft.Json;

namespace WOF.Application.CompletedOrders.EventHandlers;

public class CompletedOrderProductCreatedEventHandler : INotificationHandler<CompletedOrderProductCreatedEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<CompletedOrderProductCreatedEventHandler> _logger;
    private readonly IWalmartApiService _walmartApiService;

    public CompletedOrderProductCreatedEventHandler(ILogger<CompletedOrderProductCreatedEventHandler> logger, IApplicationDbContext context, IWalmartApiService walmartApiService)
    {
        _logger = logger;
        _context = context;
        _walmartApiService = walmartApiService;
    }

    public Task Handle(CompletedOrderProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);
        var searchResponse = _walmartApiService.Search(notification.CompletedOrderProduct.Name);

        notification.CompletedOrderProduct.WalmartSearchResponse = JsonConvert.SerializeObject(searchResponse);
        return _context.SaveChangesAsync(cancellationToken);
    }
}
