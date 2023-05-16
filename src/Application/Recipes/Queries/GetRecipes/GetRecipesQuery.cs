using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using WOF.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Products;

namespace WOF.Application.Recipes.Queries.GetRecipes;

[Authorize]
public record GetRecipesQuery : IRequest<RecipesVm>;

public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, RecipesVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetRecipesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RecipesVm> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
    {
        return new RecipesVm
        {
            UnitTypes = Enum.GetValues(typeof(UnitType))
                .Cast<UnitType>()
                .Select(p => new UnitTypeDto { Value = (int)p, Name = p.ToString() })
                .ToList(),

            Recipes = await _context.Recipes
                .AsNoTracking()
                .ProjectTo<RecipeDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
