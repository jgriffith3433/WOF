using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Ingredients.Queries.GetIngredients;

[Authorize]
public record GetIngredientsQuery : IRequest<GetIngredientsVm>;

public class GetIngredientsQueryHandler : IRequestHandler<GetIngredientsQuery, GetIngredientsVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetIngredientsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetIngredientsVm> Handle(GetIngredientsQuery request, CancellationToken cancellationToken)
    {
        return new GetIngredientsVm
        {
            Ingredients = await _context.Ingredients
                .AsNoTracking()
                .ProjectTo<IngredientBriefDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken)
        };
    }
}
