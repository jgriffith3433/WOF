using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using WOF.Domain.Enums;
using MediatR;
using AutoMapper;

namespace WOF.Application.CalledIngredients.Commands.UpdateCalledIngredientDetail;

public record UpdateCalledIngredientDetailsCommand : IRequest<CalledIngredientDetailsVm>
{
    public int Id { get; init; }

    public SizeType SizeType { get; init; }

    public int? ProductStockId { get; init; }

    public string? Name { get; init; }

    public float Units { get; init; }
}

public class UpdateCalledIngredientDetailCommandHandler : IRequestHandler<UpdateCalledIngredientDetailsCommand, CalledIngredientDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateCalledIngredientDetailCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CalledIngredientDetailsVm> Handle(UpdateCalledIngredientDetailsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.CalledIngredients
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(CalledIngredient), request.Id);
        }

        entity.Name = request.Name;
        entity.Units = request.Units;
        entity.SizeType = request.SizeType;
        if (request.ProductStockId != null)
        {
            entity.ProductStock = _context.ProductStocks.FirstOrDefault(ps => ps.Id == request.ProductStockId);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CalledIngredientDetailsVm>(entity);
    }
}
