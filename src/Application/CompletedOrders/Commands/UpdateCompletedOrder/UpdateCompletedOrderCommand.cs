using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.CompletedOrders.Commands.UpdateCompletedOrders;

public record UpdateCompletedOrderCommand : IRequest
{
    public int Id { get; init; }

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

        entity.UserImport = request.UserImport;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
