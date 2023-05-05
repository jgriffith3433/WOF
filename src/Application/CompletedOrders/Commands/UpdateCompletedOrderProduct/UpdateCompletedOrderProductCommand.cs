using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Domain.Events;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using AutoMapper;

namespace WOF.Application.CompletedOrders.Commands.UpdateCompletedOrderProduct;

public record UpdateCompletedOrderProductCommand : IRequest<CompletedOrderProductDto>
{
    public int Id { get; init; }

    public string? Name { get; init; }

    public long? WalmartId { get; init; }
}

public class UpdateCompletedOrderProductCommandHandler : IRequestHandler<UpdateCompletedOrderProductCommand, CompletedOrderProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCompletedOrderProductCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CompletedOrderProductDto> Handle(UpdateCompletedOrderProductCommand request, CancellationToken cancellationToken)
    {
        var entity = _context.CompletedOrderProducts.FirstOrDefault(cop => cop.Id == request.Id);

        if (entity != null)
        {
            if (entity.Name != request.Name && entity.WalmartId == null)
            {
                //Still searching for product
                entity.Name = request.Name;
                entity.AddDomainEvent(new CompletedOrderProductNameUpdatedEvent(entity));
            }
            if (entity.WalmartId == null && request.WalmartId != null)
            {
                //Found walmart id by searching and user manually set the product by selecting walmart id
                entity.WalmartId = request.WalmartId;
                entity.AddDomainEvent(new CompletedOrderProductWalmartIdUpdatedEvent(entity));
            }
            else if (entity.WalmartId != null && entity.WalmartId != request.WalmartId || entity.Product == null)
            {
                //Found walmart id via import but received and error response, user manually set the product by selecting walmart id
                entity.WalmartId = request.WalmartId;
                entity.AddDomainEvent(new CompletedOrderProductWalmartIdUpdatedEvent(entity));
            }

        }
        else
        {
            throw new NotFoundException(nameof(CompletedOrderProduct), request.Id);
        }
        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<CompletedOrderProductDto>(entity);
    }
}
