using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Recipes.Queries.GetRecipes;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

[Authorize]
public record GetCalledIngredientsQuery : IRequest<GetCalledIngredientsVm>;

public class GetCalledIngredientsQueryHandler : IRequestHandler<GetCalledIngredientsQuery, GetCalledIngredientsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCalledIngredientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetCalledIngredientsVm> Handle(GetCalledIngredientsQuery request, CancellationToken cancellationToken)
    {
        return new GetCalledIngredientsVm
        {
            CalledIngredients = await _context.CalledIngredients
                .AsNoTracking()
                .ProjectTo<CalledIngredientDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
