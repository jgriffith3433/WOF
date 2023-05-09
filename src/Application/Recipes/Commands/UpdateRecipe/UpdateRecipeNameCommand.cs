using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Recipes.Queries.GetRecipes;
using AutoMapper;

namespace WOF.Application.Recipes.Commands.UpdateRecipes;

public record UpdateRecipeNameCommand : IRequest<RecipeDto>
{
    public int Id { get; init; }

    public string Name { get; init; }
}

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeNameCommand, RecipeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateRecipeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RecipeDto> Handle(UpdateRecipeNameCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Recipes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Recipe), request.Id);
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecipeDto>(entity);
    }
}
