using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Enums;
using MediatR;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CookedRecipeCalledIngredients.Commands.UpdateCookedRecipeCalledIngredientDetails;

public record UpdateCookedRecipeCalledIngredientDetailsCommand : IRequest<CookedRecipeCalledIngredientDetailsVm>
{
    public int Id { get; init; }

    public SizeType SizeType { get; init; }

    public int? ProductStockId { get; init; }

    public string? Name { get; init; }

    public float? Units { get; init; }
}

public class UpdateCookedRecipeCalledIngredientDetailsCommandHandler : IRequestHandler<UpdateCookedRecipeCalledIngredientDetailsCommand, CookedRecipeCalledIngredientDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCookedRecipeCalledIngredientDetailsCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookedRecipeCalledIngredientDetailsVm> Handle(UpdateCookedRecipeCalledIngredientDetailsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CookedRecipeCalledIngredients.Include(crci => crci.CalledIngredient).Include(ps => ps.ProductStock).FirstOrDefaultAsync(crci => crci.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CookedRecipeCalledIngredient), request.Id);
        }

        entity.Name = request.Name;
        entity.Units = request.Units.Value;
        entity.SizeType = request.SizeType;

        if (entity.CalledIngredient == null && entity.ProductStock == null && request.ProductStockId != null)
        {
            var productStock = _context.ProductStocks.Include(ps => ps.Product).FirstOrDefault(ps => ps.Id == request.ProductStockId);

            if (productStock == null)
            {
                throw new NotFoundException(nameof(ProductStock), request.Id);
            }
            entity.ProductStock = productStock;
            if (productStock.Product != null)
            {
                entity.SizeType = productStock.Product.SizeType;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CookedRecipeCalledIngredientDetailsVm>(entity);
    }
}
