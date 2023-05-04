using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.Recipes.Commands.CreateRecipes;

public record CreateRecipeCommand : IRequest<int>
{
    public string? UserImport { get; init; }
}

public class CreateRecipeCommandHandler : IRequestHandler<CreateRecipeCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateRecipeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = new Recipe();

        //TODO: fill out recipe name
        //entity.Name = request.Name;
        entity.Name = "Temp name";
        entity.UserImport = request.UserImport;

        _context.Recipes.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
