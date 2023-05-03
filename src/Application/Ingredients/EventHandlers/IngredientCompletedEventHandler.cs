using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WOF.Application.Ingredients.EventHandlers;

public class IngredientCompletedEventHandler : INotificationHandler<IngredientCompletedEvent>
{
    private readonly ILogger<IngredientCompletedEventHandler> _logger;

    public IngredientCompletedEventHandler(ILogger<IngredientCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(IngredientCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
