using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WOF.Application.Ingredients.EventHandlers;

public class IngredientCreatedEventHandler : INotificationHandler<IngredientCreatedEvent>
{
    private readonly ILogger<IngredientCreatedEventHandler> _logger;

    public IngredientCreatedEventHandler(ILogger<IngredientCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IngredientCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
