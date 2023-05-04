using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Products.Commands.CreateProduct;

public record CreateCompletedOrderProductCommand : IRequest<int>
{
    public string? Name { get; init; }
    public int? WalmartId { get; init; }
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

        var existingProduct = await _context.CompletedOrderProducts.FirstOrDefaultAsync(p => p.Name.ToLower() == request.Name.ToLower());
        if (existingProduct != null)
        {
            existingProduct.CompletedOrders.Add(completedOrder);

            await _context.SaveChangesAsync(cancellationToken);

            return existingProduct.Id;
        }
        else
        {
            var entity = new Product
            {
                Name = request.Name,
                WalmartId = request.WalmartId
            };

            entity.CompletedOrders.Add(completedOrder);

            entity.AddDomainEvent(new ProductCreatedEvent(entity));

            _context.Products.Add(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
