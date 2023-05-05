using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CompletedOrders.Commands.CreateCompletedOrderProduct;

public record CreateCompletedOrderProductCommand : IRequest<int>
{
    public string? Name { get; init; }
    public int CompletedOrderId { get; init; }
}

public class CreateCompletedOrderProductCommandHandler : IRequestHandler<CreateCompletedOrderProductCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateCompletedOrderProductCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateCompletedOrderProductCommand request, CancellationToken cancellationToken)
    {
        var completedOrder = await _context.CompletedOrders.FirstOrDefaultAsync(co => co.Id == request.CompletedOrderId);

        var entity = new CompletedOrderProduct
        {
            Name = request.Name
        };

        _context.CompletedOrderProducts.Add(entity);

        completedOrder.CompletedOrderProducts.Add(entity);

        entity.AddDomainEvent(new CompletedOrderProductCreatedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
        
    }
}
