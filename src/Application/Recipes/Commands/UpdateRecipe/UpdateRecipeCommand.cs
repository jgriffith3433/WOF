using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;

namespace WOF.Application.Recipes.Commands.UpdateRecipes;

public record UpdateRecipeCommand : IRequest
{
    public int Id { get; init; }

    public string? UserImport { get; init; }
}

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateRecipeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Recipes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Recipe), request.Id);
        }

        entity.UserImport = request.UserImport;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
