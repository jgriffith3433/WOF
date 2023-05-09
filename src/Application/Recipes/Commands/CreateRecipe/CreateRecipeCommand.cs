using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Domain.Events;
using WOF.Application.Recipes.Queries.GetRecipes;
using AutoMapper;

namespace WOF.Application.Recipes.Commands.CreateRecipes;

public record CreateRecipeCommand : IRequest<RecipeDto>
{
    public string Name { get; init; }
    public int? Serves { get; init; }
    public string? UserImport { get; init; }
}

public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, RecipeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateRecipeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RecipeDto> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Recipe();

        entity.Name = request.Name;
        entity.Serves = request.Serves.Value;
        entity.UserImport = request.UserImport;
        _context.Recipes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        if (entity.UserImport != null)
        {
            entity.AddDomainEvent(new RecipeUserImportEvent(entity));
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecipeDto>(entity);
    }
}
