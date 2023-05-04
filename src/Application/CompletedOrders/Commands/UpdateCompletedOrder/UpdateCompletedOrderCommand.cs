using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Domain.Events;

namespace WOF.Application.CompletedOrders.Commands.UpdateCompletedOrders;

public record UpdateCompletedOrderCommand : IRequest
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string? UserImport { get; init; }
}

public class UpdateCompletedOrderCommandHandler : IRequestHandler<UpdateCompletedOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCompletedOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateCompletedOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CompletedOrders
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CompletedOrder), request.Id);
        }

        entity.Name = request.Name;
        var imported = false;
        if (request.UserImport != null && entity.UserImport != request.UserImport)
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

        return Unit.Value;
    }
}
