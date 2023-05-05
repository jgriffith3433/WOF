using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace WOF.Application.CalledIngredients.EventHandlers;

public class CalledIngredientCompletedEventHandler : INotificationHandler<CalledIngredientCompletedEvent>
{
    private readonly ILogger<CalledIngredientCompletedEventHandler> _logger;

    public CalledIngredientCompletedEventHandler(ILogger<CalledIngredientCompletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CalledIngredientCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
