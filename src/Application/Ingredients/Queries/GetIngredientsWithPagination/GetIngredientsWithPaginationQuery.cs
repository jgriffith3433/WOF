using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Mappings;
using WOF.Application.Common.Models;
using MediatR;

namespace WOF.Application.Ingredients.Queries.GetIngredientsWithPagination;

public record GetIngredientsWithPaginationQuery : IRequest<PaginatedList<IngredientBriefDto>>
{
    public int ListId { get; init; }
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetIngredientsWithPaginationQueryHandler : IRequestHandler<GetIngredientsWithPaginationQuery, PaginatedList<IngredientBriefDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetIngredientsWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<IngredientBriefDto>> Handle(GetIngredientsWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.Ingredients
            //TODO: look into this
            //.Where(x => x.ListId == request.ListId)
            .OrderBy(x => x.Name)
            .ProjectTo<IngredientBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
