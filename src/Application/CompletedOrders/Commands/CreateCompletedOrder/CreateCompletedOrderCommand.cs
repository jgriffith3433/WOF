using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.CompletedOrders.Commands.CreateCompletedOrders;

public record CreateCompletedOrderCommand : IRequest<int>
{
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

        entity.UserImport = request.UserImport;

        _context.CompletedOrders.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
