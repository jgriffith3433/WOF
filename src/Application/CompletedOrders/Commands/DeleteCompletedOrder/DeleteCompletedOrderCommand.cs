using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CompletedOrders.Commands.DeleteCompletedOrders;

public record DeleteCompletedOrderCommand(int Id) : IRequest;

public class DeleteCompletedOrderCommandHandler : IRequestHandler<DeleteCompletedOrderCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCompletedOrderCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteCompletedOrderCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CompletedOrders
            .Where(l => l.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CompletedOrder), request.Id);
        }

        _context.CompletedOrders.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
