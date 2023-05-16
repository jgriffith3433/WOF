using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Products.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WOF.Domain.Enums;

namespace WOF.Application.Products.Commands.UpdateProduct;

public record UpdateProductUnitTypeCommand : IRequest<ProductDto>
{
    public int Id { get; init; }

    public int UnitType { get; init; }
}

public class UpdateProductUnitTypeCommandHandler : IRequestHandler<UpdateProductUnitTypeCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProductUnitTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductUnitTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.Include(p => p.ProductStock).FirstOrDefaultAsync(ps => ps.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        entity.UnitType = (UnitType)request.UnitType;

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}
