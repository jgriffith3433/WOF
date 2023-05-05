using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

[Authorize]
public record GetCalledIngredientDetailsQuery : IRequest<CalledIngredientDetailsVm>
{
    public int Id { get; set; }
}

public class GetCalledIngredientDetailsQueryHandler : IRequestHandler<GetCalledIngredientDetailsQuery, CalledIngredientDetailsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCalledIngredientDetailsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CalledIngredientDetailsVm> Handle(GetCalledIngredientDetailsQuery request, CancellationToken cancellationToken)
    {
        var calledIngredientDetails = await _context.CalledIngredients.AsNoTracking().ProjectTo<CalledIngredientDetailsVm>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);

        var query = from ps in _context.ProductStocks
                    where EF.Functions.Like(ps.Name, string.Format("%{0}%", calledIngredientDetails.Name))
                    select ps;

        calledIngredientDetails.ProductStockSearchItems = query.ToList();

        return calledIngredientDetails;
    }
}
