using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

[Authorize]
public record GetCalledIngredientQuery : IRequest<CalledIngredientDto>
{
    public int Id { get; set; }
}

public class GetCalledIngredientQueryHandler : IRequestHandler<GetCalledIngredientQuery, CalledIngredientDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCalledIngredientQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CalledIngredientDto> Handle(GetCalledIngredientQuery request, CancellationToken cancellationToken)
    {
        return await _context.CalledIngredients.AsNoTracking().ProjectTo<CalledIngredientDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(ci => ci.Id == request.Id);
    }
}
