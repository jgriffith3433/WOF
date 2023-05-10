using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using MediatR;
using WOF.Application.Common.Security;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CookedRecipeCalledIngredients.Queries.GetCookedRecipeCalledIngredients;

[Authorize]
public record SearchProductStockNameQuery : IRequest<CookedRecipeCalledIngredientDetailsVm>
{
    public int Id { get; init; }
    public string Search { get; init; }
}

public class SearchCompletedOrderProductNameQueryHandler : IRequestHandler<SearchProductStockNameQuery, CookedRecipeCalledIngredientDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchCompletedOrderProductNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookedRecipeCalledIngredientDetailsVm> Handle(SearchProductStockNameQuery request, CancellationToken cancellationToken)
    {
        var cookedRecipeCalledIngredientDetails = await _context.CookedRecipeCalledIngredients.AsNoTracking().ProjectTo<CookedRecipeCalledIngredientDetailsVm>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);

        var query = from ps in _context.ProductStocks
                    where EF.Functions.Like(ps.Name, string.Format("%{0}%", request.Search))
                    select ps;

        cookedRecipeCalledIngredientDetails.ProductStockSearchItems = query.ToList();

        return cookedRecipeCalledIngredientDetails;
    }
}
