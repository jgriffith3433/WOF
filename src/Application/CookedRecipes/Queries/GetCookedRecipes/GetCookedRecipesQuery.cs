﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using WOF.Application.Common.Interfaces;
using WOF.Application.Common.Security;
using WOF.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WOF.Application.Products;

namespace WOF.Application.CookedRecipes.Queries.GetCookedRecipes;

[Authorize]
public record GetCookedRecipesQuery : IRequest<CookedRecipesVm>;

public class GetCookedRecipesQueryHandler : IRequestHandler<GetCookedRecipesQuery, CookedRecipesVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetCookedRecipesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookedRecipesVm> Handle(GetCookedRecipesQuery request, CancellationToken cancellationToken)
    {
        return new CookedRecipesVm
        {
            SizeTypes = Enum.GetValues(typeof(SizeType))
                .Cast<SizeType>()
                .Select(p => new SizeTypeDto { Value = (int)p, Name = p.ToString() })
                .ToList(),

            CookedRecipes = await _context.CookedRecipes
                .AsNoTracking()
                .ProjectTo<CookedRecipeDto>(_mapper.ConfigurationProvider)
                .OrderBy(t => t.Id)
                .ToListAsync(cancellationToken),

            RecipesOptions = _context.Recipes.Select(r => new RecipesOptionVm { Value = r.Id, Name = r.Name }).ToList(),
        };
    }
}
