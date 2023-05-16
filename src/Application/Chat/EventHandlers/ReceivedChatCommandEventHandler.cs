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
using WOF.Domain.Enums;
using System.Drawing;
using System.Text;

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
        try
        {
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

                        //ingredients can be CalledIngredients or CookedRecipeCalledIngredients in the context of substituting or changing recipe ingredients
                        switch (notification.ChatCommand.CurrentUrl)
                        {
                            case "recipes":
                            case "called-ingredients":
                                {
                                    var recipe = _context.Recipes.Include(r => r.CalledIngredients).ThenInclude(ci => ci.ProductStock).FirstOrDefault(r => r.Name.ToLower() == substituteIngredient.Recipe.ToLower());
                                    if (recipe == null)
                                    {
                                        systemResponse = "Could not find recipe by name: " + substituteIngredient.Recipe;
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
                                            calledIngredient.ProductStock = null;
                                        }
                                    }

                                    break;
                                }
                            case "cooked-recipes":
                                {
                                    var cookedRecipe = _context.CookedRecipes.Include(cr => cr.Recipe).Include(cr => cr.CookedRecipeCalledIngredients).ThenInclude(crci => crci.CalledIngredient).Include(cr => cr.CookedRecipeCalledIngredients).ThenInclude(crci => crci.ProductStock).OrderByDescending(cr => cr.Created).FirstOrDefault(cr => cr.Recipe.Name.ToLower().Contains(substituteIngredient.Recipe.ToLower()));
                                    if (cookedRecipe == null)
                                    {
                                        systemResponse = "Could not find cooked recipe by name: " + substituteIngredient.Recipe;
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(substituteIngredient.Original))
                                        {
                                            systemResponse = "original property is undefined, cannot edit cooked recipe ingredient";
                                        }
                                        else
                                        {
                                            var cookedRecipeCalledIngredient = cookedRecipe.CookedRecipeCalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(substituteIngredient.Original.ToLower()));

                                            if (cookedRecipeCalledIngredient == null)
                                            {
                                                systemResponse = "Could not find ingredient by name: " + substituteIngredient.Original;
                                            }
                                            else
                                            {
                                                cookedRecipeCalledIngredient.Name = substituteIngredient.New;
                                                cookedRecipeCalledIngredient.CalledIngredient = null;
                                                cookedRecipeCalledIngredient.ProductStock = null;
                                            }
                                        }
                                    }
                                    break;
                                }
                            default:
                                systemResponse = "Unsure if the user is changing a recipe ingredient or changing a logged recipe ingredient.";
                                break;
                        }
                        break;
                    }
                case "add-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var addRecipeIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandAddRecipeIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        //ingredients can be CalledIngredients or CookedRecipeCalledIngredients in the context of adding recipe ingredients
                        switch (notification.ChatCommand.CurrentUrl)
                        {
                            case "recipes":
                            case "called-ingredients":
                                {
                                    var recipe = _context.Recipes.Include(r => r.CalledIngredients).FirstOrDefault(r => r.Name.ToLower() == addRecipeIngredient.Recipe.ToLower());
                                    if (recipe == null)
                                    {
                                        systemResponse = "Could not find recipe by name: " + addRecipeIngredient.Recipe;
                                    }
                                    else
                                    {
                                        var calledIngredient = new CalledIngredient
                                        {
                                            Name = addRecipeIngredient.Name,
                                            Recipe = recipe,
                                            Verified = false,
                                            Units = addRecipeIngredient.Units,
                                            UnitType = UnitTypeFromString(addRecipeIngredient.UnitType)
                                        };
                                        recipe.CalledIngredients.Add(calledIngredient);
                                        _context.CalledIngredients.Add(calledIngredient);
                                    }
                                    break;
                                }
                            case "cooked-recipes":
                                {
                                    var cookedRecipe = _context.CookedRecipes.Include(r => r.CookedRecipeCalledIngredients).OrderByDescending(cr => cr.Created).FirstOrDefault(r => r.Recipe.Name.ToLower() == addRecipeIngredient.Recipe.ToLower());
                                    if (cookedRecipe == null)
                                    {
                                        systemResponse = "Could not find cooked recipe by name: " + addRecipeIngredient.Recipe;
                                    }
                                    else
                                    {
                                        var cookedRecipeCalledIngredient = new CookedRecipeCalledIngredient
                                        {
                                            Name = addRecipeIngredient.Name,
                                            CookedRecipe = cookedRecipe,
                                            Units = addRecipeIngredient.Units,
                                            UnitType = UnitTypeFromString(addRecipeIngredient.UnitType)
                                        };
                                        cookedRecipe.CookedRecipeCalledIngredients.Add(cookedRecipeCalledIngredient);
                                        _context.CookedRecipeCalledIngredients.Add(cookedRecipeCalledIngredient);
                                    }
                                    break;
                                }
                            default:
                                systemResponse = "Unsure if the user is adding a recipe ingredient or adding a cooked recipe ingredient.";
                                break;
                        }
                        break;
                    }
                case "edit-recipe-ingredient-unittype":
                case "edit-ingredient-unittype":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var editIngredientUnitType = JsonConvert.DeserializeObject<OpenApiChatCommandEditIngredientUnitType>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        //ingredients can be CalledIngredients, CookedRecipeCalledIngredients, and Products in the context of changing the unit type
                        switch (notification.ChatCommand.CurrentUrl)
                        {
                            case "recipes":
                            case "called-ingredients":
                                {
                                    var calledIngredient = _context.CalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(editIngredientUnitType.Name.ToLower()));
                                    if (calledIngredient == null)
                                    {
                                        systemResponse = "Could not find ingredient by name: " + editIngredientUnitType.Name;
                                    }
                                    else
                                    {
                                        calledIngredient.UnitType = UnitTypeFromString(editIngredientUnitType.UnitType);
                                    }
                                    break;
                                }
                            case "cooked-recipes":
                                {
                                    var cookedRecipeCalledIngredient = _context.CookedRecipeCalledIngredients.FirstOrDefault(crci => crci.Name.ToLower().Contains(editIngredientUnitType.Name.ToLower()));
                                    if (cookedRecipeCalledIngredient == null)
                                    {
                                        systemResponse = "Could not find ingredient by name: " + editIngredientUnitType.Name;
                                    }
                                    else
                                    {
                                        cookedRecipeCalledIngredient.UnitType = UnitTypeFromString(editIngredientUnitType.UnitType);
                                    }
                                    break;
                                }
                            case "completedorders":
                            case "products":
                            case "product-stock":
                                {
                                    var product = _context.Products.FirstOrDefault(ci => ci.Name.ToLower().Contains(editIngredientUnitType.Name.ToLower()));
                                    if (product == null)
                                    {
                                        systemResponse = "Could not find ingredient by name: " + editIngredientUnitType.Name;
                                    }
                                    else
                                    {
                                        product.UnitType = UnitTypeFromString(editIngredientUnitType.UnitType);
                                    }
                                    break;
                                }
                            default:
                                systemResponse = "Could not find ingredient by name: " + editIngredientUnitType.Name;
                                break;
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

                        foreach (var createRecipeIngredient in createRecipe.Ingredients)
                        {
                            var calledIngredient = new CalledIngredient
                            {
                                Name = createRecipeIngredient.Name,
                                Recipe = recipeEntity,
                                Verified = false,
                                Units = createRecipeIngredient.Units,
                                UnitType = UnitTypeFromString(createRecipeIngredient.UnitType)
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
                case "cook-recipe":
                case "log-cooked-recipe":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var createCookedRecipe = JsonConvert.DeserializeObject<OpenApiChatCommandCreateCookedRecipe>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var recipe = _context.Recipes
                            .Where(r => r.Name.ToLower() == createCookedRecipe.Name.ToLower()).Include(r => r.CalledIngredients).ThenInclude(ci => ci.ProductStock)
                            .SingleOrDefaultAsync(cancellationToken).Result;

                        if (recipe == null)
                        {
                            systemResponse = "Could not find recipe by name: " + createCookedRecipe.Name;
                        }
                        else
                        {
                            var cookedRecipe = new CookedRecipe
                            {
                                Recipe = recipe
                            };
                            foreach (var calledIngredient in recipe.CalledIngredients)
                            {
                                var cookedRecipeCalledIngredient = new CookedRecipeCalledIngredient
                                {
                                    Name = calledIngredient.Name,
                                    CookedRecipe = cookedRecipe,
                                    CalledIngredient = calledIngredient,
                                    ProductStock = calledIngredient.ProductStock,
                                    UnitType = calledIngredient.UnitType,
                                    Units = calledIngredient.Units != null ? calledIngredient.Units.Value : 0
                                };
                                cookedRecipe.CookedRecipeCalledIngredients.Add(cookedRecipeCalledIngredient);
                            }
                            _context.CookedRecipes.Add(cookedRecipe);
                        }

                        break;
                    }
                default:
                    {
                        notification.ChatCommand.Unknown = true;
                    }
                    break;
            }
            notification.ChatCommand.ChangedData = _context.ChangeTracker.HasChanges();
            notification.ChatCommand.SystemResponse = systemResponse;
        }
        catch (Exception ex)
        {
            notification.ChatCommand.Error = FlattenException(ex);
            notification.ChatCommand.SystemResponse = "An error occured: " + ex.Message;
        }
        return _context.SaveChangesAsync(cancellationToken);
    }

    private string FlattenException(Exception exception)
    {
        var stringBuilder = new StringBuilder();

        while (exception != null)
        {
            stringBuilder.AppendLine(exception.Message);
            stringBuilder.AppendLine(exception.StackTrace);

            exception = exception.InnerException;
        }

        return stringBuilder.ToString();
    }

    private UnitType UnitTypeFromString(string unitTypeStr)
    {
        switch (unitTypeStr.ToLower())
        {
            case "":
            case "none":
                return UnitType.None;
            case "bulk":
                return UnitType.Bulk;
            case "ounce":
            case "ounces":
                return UnitType.Ounce;
            case "teaspoon":
            case "teaspoons":
                return UnitType.Teaspoon;
            case "tablespoon":
            case "tablespoons":
                return UnitType.Tablespoon;
            case "pound":
            case "pounds":
                return UnitType.Pound;
            case "cup":
            case "cups":
                return UnitType.Cup;
            case "clove":
            case "cloves":
                return UnitType.Cloves;
            case "can":
            case "cans":
                return UnitType.Can;
            case "whole":
            case "wholes":
                return UnitType.Whole;
            case "package":
            case "packages":
                return UnitType.Package;
            case "bar":
            case "bars":
                return UnitType.Bar;
            case "bun":
            case "buns":
                return UnitType.Bun;
            default:
                return UnitType.None;
        }
    }
}
