using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Domain.Events;

namespace WOF.Application.CompletedOrders.Commands.CreateCompletedOrders;

public record CreateCompletedOrderCommand : IRequest<int>
{
    public string Name { get; init; }
    public string? UserImport { get; init; }
}

public class CreateCompletedOrderCommandHandler : IRequestHandler<CreateCompletedOrderCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCompletedOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCompletedOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = new CompletedOrder();

        entity.Name = request.Name;

        _context.CompletedOrders.Add(entity);

        var imported = false;
        if (request.UserImport != null)
        {
            imported = true;
        }

        entity.UserImport = request.UserImport;

        await _context.SaveChangesAsync(cancellationToken);

        if (imported)
        {
            entity.AddDomainEvent(new CompletedOrderUserImportEvent(entity));
        }

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
