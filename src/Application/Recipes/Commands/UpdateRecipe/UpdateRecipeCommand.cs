using WOF.Application.Common.Exceptions;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Domain.Events;
using WOF.Application.Recipes.Queries.GetRecipes;
using AutoMapper;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;

namespace WOF.Application.Recipes.Commands.UpdateRecipes;

public record UpdateRecipeCommand : IRequest<RecipeDto>
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string? UserImport { get; init; }
}

public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, RecipeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public UpdateRecipeCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RecipeDto> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Recipes
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Recipe), request.Id);
        }

        entity.Name = request.Name;
        entity.UserImport = request.UserImport;

        await _context.SaveChangesAsync(cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecipeDto>(entity);
    }
}
