using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Common.Interfaces;
using WOF.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace WOF.Application.Recipes.EventHandlers;

public class RecipeUserImportEventHandler : INotificationHandler<RecipeUserImportEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<RecipeUserImportEventHandler> _logger;

    public RecipeUserImportEventHandler(ILogger<RecipeUserImportEventHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public Task Handle(RecipeUserImportEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);

        //create called ingredients per recipe user import
        var userImportNewLineSplit = notification.Recipe.UserImport.Split('\n');

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
                notification.Recipe.CalledIngredients.Add(calledIngredient);
            }
        }

        return _context.SaveChangesAsync(cancellationToken);
    }
}
