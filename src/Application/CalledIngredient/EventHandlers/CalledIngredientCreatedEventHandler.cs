using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WOF.Application.CalledIngredients.EventHandlers;

public class CalledIngredientCreatedEventHandler : INotificationHandler<CalledIngredientCreatedEvent>
{
    private readonly ILogger<CalledIngredientCreatedEventHandler> _logger;

    public CalledIngredientCreatedEventHandler(ILogger<CalledIngredientCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CalledIngredientCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
