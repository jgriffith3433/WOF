using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.CookedRecipes.Queries.GetCookedRecipes;
using AutoMapper;
using WOF.Application.Common.Exceptions;

namespace WOF.Application.CookedRecipes.Commands.CreateCookedRecipes;

public record CreateCookedRecipeCommand : IRequest<CookedRecipeDto>
{
    public int? RecipeId { get; init; }
}

public class CreateCookedRecipeCommandHandler : IRequestHandler<CreateCookedRecipeCommand, CookedRecipeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCookedRecipeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CookedRecipeDto> Handle(CreateCookedRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = new CookedRecipe();
        var recipeEntity = _context.Recipes.FirstOrDefault(r => r.Id == request.RecipeId);
        if (recipeEntity == null)
        {
            throw new NotFoundException(nameof(Recipe), request.RecipeId);
        }
        entity.Recipe = recipeEntity;

        _context.CookedRecipes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CookedRecipeDto>(entity);
    }
}
