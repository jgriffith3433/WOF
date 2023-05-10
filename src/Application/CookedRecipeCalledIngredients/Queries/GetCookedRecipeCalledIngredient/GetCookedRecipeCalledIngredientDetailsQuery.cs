using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CookedRecipeCalledIngredients.Queries.GetCookedRecipeCalledIngredients;

[Authorize]
public record GetCookedRecipeCalledIngredientDetailsQuery : IRequest<CookedRecipeCalledIngredientDetailsVm>
{
    public int Id { get; set; }
}

public class GetCookedRecipeCalledIngredientDetailsQueryHandler : IRequestHandler<GetCookedRecipeCalledIngredientDetailsQuery, CookedRecipeCalledIngredientDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCookedRecipeCalledIngredientDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookedRecipeCalledIngredientDetailsVm> Handle(GetCookedRecipeCalledIngredientDetailsQuery request, CancellationToken cancellationToken)
    {
        var cookedRecipeCalledIngredientDetails = await _context.CookedRecipeCalledIngredients.AsNoTracking().ProjectTo<CookedRecipeCalledIngredientDetailsVm>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);

        var query = from ps in _context.ProductStocks
                    where EF.Functions.Like(ps.Name, string.Format("%{0}%", cookedRecipeCalledIngredientDetails.Name))
                    select ps;

        cookedRecipeCalledIngredientDetails.ProductStockSearchItems = query.ToList();

        return cookedRecipeCalledIngredientDetails;
    }
}
