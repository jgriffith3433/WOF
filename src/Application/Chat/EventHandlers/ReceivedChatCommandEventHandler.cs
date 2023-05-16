using WOF.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using WOF.Application.Common.Interfaces;
using Newtonsoft.Json;
using WOF.Application.Chat.Queries.GetResponse;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WOF.Application.Recipes.Commands.CreateRecipes;
using Org.BouncyCastle.Asn1.Ocsp;
using WOF.Domain.Entities;
using WOF.Application.Common.Exceptions;

namespace WOF.Application.Products.EventHandlers;

public class ReceivedChatCommandEventHandler : INotificationHandler<ReceivedChatCommandEvent>
{
    private readonly IApplicationDbContext _context;
    private readonly ILogger<ReceivedChatCommandEventHandler> _logger;
    private readonly IMediator _mediator;

    public ReceivedChatCommandEventHandler(ILogger<ReceivedChatCommandEventHandler> logger, IApplicationDbContext context, IMediator mediator)
    {
        _logger = logger;
        _context = context;
        _mediator = mediator;
    }

    public Task Handle(ReceivedChatCommandEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("WOF Domain Event: {DomainEvent}", notification.GetType().Name);
        string? systemResponse = null;
        switch (notification.ChatCommand.CommandName.ToLower())
        {
            case "order":
                {
                    var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                    var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                    var chatCommandOrder = JsonConvert.DeserializeObject<OpenApiChatCommandOrder>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                    break;
                }
            case "edit-recipe-name":
                {
                    var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                    var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                    var editRecipeName = JsonConvert.DeserializeObject<OpenApiChatCommandEditRecipeName>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                    var recipe = _context.Recipes.FirstOrDefault(r => r.Name.ToLower() == editRecipeName.Original.ToLower());
                    if (recipe == null)
                    {
                        systemResponse = "Could not find recipe by name: " + editRecipeName.Original;
                    }
                    else
                    {
                        recipe.Name = editRecipeName.New;
                    }

                    break;
                }
            case "substitute-ingredient":
            case "edit-recipe-ingredient":
                {
                    var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                    var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                    var substituteIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandSubstituteIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                    var recipe = _context.Recipes.Include(r => r.CalledIngredients).FirstOrDefault(r => r.Name.ToLower() == substituteIngredient.Recipe.ToLower());
                    if (recipe == null)
                    {
                        systemResponse = "Could not find ingredient by name: " + substituteIngredient.Recipe;
                    }
                    else
                    {
                        var calledIngredient = recipe.CalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(substituteIngredient.Original.ToLower()));

                        if (calledIngredient == null)
                        {
                            systemResponse = "Could not find ingredient by name: " + substituteIngredient.Original;
                        }
                        else
                        {
                            calledIngredient.Name = substituteIngredient.New;
                        }
                    }

                    break;
                }
            case "create-recipe":
                {
                    var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                    var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                    var createRecipe = JsonConvert.DeserializeObject<OpenApiChatCommandCreateRecipe>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                    //var response = _mediator.Send(new CreateRecipeCommand
                    //{
                    //    Name = createRecipe.Name,
                    //    UserImport = notification.ChatCommand.RawReponse
                    //});
                    var recipeEntity = new Recipe();

                    recipeEntity.Name = createRecipe.Name;
                    //entity.Serves = createRecipe.Serves.Value;
                    recipeEntity.UserImport = notification.ChatCommand.RawReponse;

                    foreach(var createRecipeIngredient in createRecipe.Ingredients)
                    {
                        var calledIngredient = new CalledIngredient
                        {
                            Name = createRecipeIngredient.Name,
                            Recipe = recipeEntity,
                            Verified = false,
                            SizeType = Domain.Enums.SizeType.None
                        };
                        recipeEntity.CalledIngredients.Add(calledIngredient);
                    }

                    _context.Recipes.Add(recipeEntity);


                    //if (entity.UserImport != null)
                    //{
                    //    entity.AddDomainEvent(new RecipeUserImportEvent(entity));
                    //}
                    //await _context.SaveChangesAsync(cancellationToken);



                }
                break;
            case "delete-recipe":
                {
                    var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                    var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                    var deleteRecipe = JsonConvert.DeserializeObject<OpenApiChatCommandDeleteRecipe>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                    var recipe = _context.Recipes
                        .Where(r => r.Name.ToLower() == deleteRecipe.Name.ToLower()).Include(r => r.CalledIngredients)
                        .SingleOrDefaultAsync(cancellationToken).Result;

                    if (recipe == null)
                    {
                        systemResponse = "Could not find recipe by name: " + deleteRecipe.Name;
                    }
                    else
                    {

                        foreach (var calledIngredient in recipe.CalledIngredients)
                        {
                            _context.CalledIngredients.Remove(calledIngredient);
                        }
                        _context.Recipes.Remove(recipe);
                    }
                }
                break;
            default:
                {
                    notification.ChatCommand.Unknown = true;
                }
                break;
        }
        notification.ChatCommand.ChangedData = _context.ChangeTracker.HasChanges();
        notification.ChatCommand.SystemResponse = systemResponse;
        return _context.SaveChangesAsync(cancellationToken);
    }
}
