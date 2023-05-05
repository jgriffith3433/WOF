using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using MediatR;
using WOF.Application.Walmart.Queries;
using WOF.Application.Walmart.Requests;
using WOF.Application.Walmart.Responses;
using WOF.Application.Common.Exceptions;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Numerics;
using Org.BouncyCastle.Crypto;
using WOF.Application.Recipes.Queries.GetRecipes;

namespace WOF.Application.Walmart.Commands;


public record CreateCalledIngredientsFromRecipeCommand : IRequest<RecipeDto>
{
    public int RecipeId { get; init; }
}

public class CreateCalledIngredientsFromCompletedOrderCommandHandler : IRequestHandler<CreateCalledIngredientsFromRecipeCommand, RecipeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CreateCalledIngredientsFromCompletedOrderCommandHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<RecipeDto> Handle(CreateCalledIngredientsFromRecipeCommand request, CancellationToken cancellationToken)
    {
        var recipe = await _context.Recipes
            .FindAsync(new object[] { request.RecipeId }, cancellationToken);

        if (recipe == null)
        {
            throw new NotFoundException(nameof(CompletedOrder), request.RecipeId);
        }

        //create called ingredients per recipe user import
        var userImportNewLineSplit = recipe.UserImport.Split('\n');

        foreach (var l in userImportNewLineSplit)
        {
            var line = l.Trim();
            if (line.Length > 0)
            {
                ProductStock foundProductStock = null;
                //try to match stock ingredient by name
                var wordsInLine = line.Split(' ');
                foreach (var w in wordsInLine)
                {
                    var word = w.Trim();
                    //skip any words that have numbers in them
                    if (!word.Any(char.IsDigit))
                    {
                        var query = from ps in _context.ProductStocks
                                    where EF.Functions.Like(ps.Name, string.Format("%{0}%", word))
                                    select ps;

                        var productStock = query.FirstOrDefault();

                        if (productStock != null)
                        {
                            //foundProductStock = productStock;
                            break;
                        }
                        else
                        {
                            //TODO: search from walmart?
                        }
                    }
                }

                var calledIngredient = new CalledIngredient
                {
                    Name = line
                };
                if (foundProductStock != null)
                {
                    calledIngredient.ProductStock = foundProductStock;
                }
                _context.CalledIngredients.Add(calledIngredient);
                recipe.CalledIngredients.Add(calledIngredient);
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<RecipeDto>(recipe);
    }
}

