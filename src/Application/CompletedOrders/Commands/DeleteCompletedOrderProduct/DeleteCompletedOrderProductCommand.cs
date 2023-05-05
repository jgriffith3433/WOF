using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;

namespace WOF.Application.CompletedOrders.Commands.DeleteCompletedOrderProduct;

public record DeleteCompletedOrderProductCommand(int Id) : IRequest;

public class DeleteCompletedOrderProductCommandHandler : IRequestHandler<DeleteCompletedOrderProductCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompletedOrderProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCompletedOrderProductCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CompletedOrderProducts
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        _context.CompletedOrderProducts.Remove(entity);

        entity.AddDomainEvent(new CompletedOrderProductDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
