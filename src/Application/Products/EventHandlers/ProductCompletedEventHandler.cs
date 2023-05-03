using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WOF.Application.Products.EventHandlers;

public class ProductCompletedEventHandler : INotificationHandler<ProductCompletedEvent>
{
    private readonly ILogger<ProductCompletedEventHandler> _logger;

    public ProductCompletedEventHandler(ILogger<ProductCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ProductCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
