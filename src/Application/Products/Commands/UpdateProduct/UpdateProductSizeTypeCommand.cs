using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Products.Queries;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WOF.Domain.Enums;

namespace WOF.Application.Products.Commands.UpdateProduct;

public record UpdateProductSizeTypeCommand : IRequest<ProductDto>
{
    public int Id { get; init; }

    public int SizeType { get; init; }
}

public class UpdateProductSizeTypeCommandHandler : IRequestHandler<UpdateProductSizeTypeCommand, ProductDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateProductSizeTypeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(UpdateProductSizeTypeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Products.Include(p => p.ProductStock).FirstOrDefaultAsync(ps => ps.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        entity.SizeType = (SizeType)request.SizeType;

        await _context.SaveChangesAsync(cancellationToken);
        return _mapper.Map<ProductDto>(entity);
    }
}
