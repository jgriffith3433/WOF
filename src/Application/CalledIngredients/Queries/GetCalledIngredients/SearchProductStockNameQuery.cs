using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using MediatR;
using WOF.Application.Common.Security;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

[Authorize]
public record SearchProductStockNameQuery : IRequest<CalledIngredientDetailsVm>
{
    public int Id { get; init; }
    public string Search { get; init; }
}

public class SearchCompletedOrderProductNameQueryHandler : IRequestHandler<SearchProductStockNameQuery, CalledIngredientDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public SearchCompletedOrderProductNameQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CalledIngredientDetailsVm> Handle(SearchProductStockNameQuery request, CancellationToken cancellationToken)
    {
        var calledIngredientDetails = await _context.CalledIngredients.AsNoTracking().ProjectTo<CalledIngredientDetailsVm>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);

        var query = from ps in _context.ProductStocks
                    where EF.Functions.Like(ps.Name, string.Format("%{0}%", request.Search))
                    select ps;

        calledIngredientDetails.ProductStockSearchItems = query.ToList();

        return calledIngredientDetails;
    }
}
