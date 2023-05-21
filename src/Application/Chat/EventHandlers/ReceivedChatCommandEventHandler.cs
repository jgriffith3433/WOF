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

                    }
                    break;
                case "edit-recipe-name":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var editRecipeName = JsonConvert.DeserializeObject<OpenApiChatCommandEditRecipeName>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var recipe = _context.Recipes.FirstOrDefault(r => r.Name.ToLower().Contains(editRecipeName.Original.ToLower()));
                        if (recipe == null)
                        {
                            systemResponse = "Error: Could not find recipe by name: " + editRecipeName.Original;
                        }
                        else
                        {
                            recipe.Name = editRecipeName.New;
                        }

                    }
                    break;
                case "substitute-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var recipeSubstituteIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandCookedRecipeSubstituteIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));


                        var recipe = _context.Recipes.Include(r => r.CalledIngredients).ThenInclude(ci => ci.ProductStock).FirstOrDefault(r => r.Name.ToLower() == recipeSubstituteIngredient.Recipe.ToLower());
                        if (recipe == null)
                        {
                            systemResponse = "Error: Could not find recipe by name: " + recipeSubstituteIngredient.Recipe;
                        }
                        else
                        {
                            var calledIngredient = recipe.CalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(recipeSubstituteIngredient.Original.ToLower()));

                            if (calledIngredient == null)
                            {
                                systemResponse = "Error: Could not find ingredient by name: " + recipeSubstituteIngredient.Original;
                            }
                            else
                            {
                                calledIngredient.Name = recipeSubstituteIngredient.New;
                                calledIngredient.ProductStock = null;
                            }
                        }
                    }
                    break;
                case "substitute-cooked-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var cookedRecipeSubstituteIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandCookedRecipeSubstituteIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));


                        var cookedRecipe = _context.CookedRecipes.Include(cr => cr.Recipe).Include(cr => cr.CookedRecipeCalledIngredients).ThenInclude(crci => crci.CalledIngredient).Include(cr => cr.CookedRecipeCalledIngredients).ThenInclude(crci => crci.ProductStock).OrderByDescending(cr => cr.Created).FirstOrDefault(cr => cr.Recipe.Name.ToLower().Contains(cookedRecipeSubstituteIngredient.Recipe.ToLower()));
                        if (cookedRecipe == null)
                        {
                            systemResponse = "Error: Could not find cooked recipe by name: " + cookedRecipeSubstituteIngredient.Recipe;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(cookedRecipeSubstituteIngredient.Original))
                            {
                                systemResponse = "Error: Original property is undefined, cannot edit cooked recipe ingredient";
                            }
                            else
                            {
                                var cookedRecipeCalledIngredient = cookedRecipe.CookedRecipeCalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(cookedRecipeSubstituteIngredient.Original.ToLower()));

                                if (cookedRecipeCalledIngredient == null)
                                {
                                    systemResponse = "Error: Could not find ingredient by name: " + cookedRecipeSubstituteIngredient.Original;
                                }
                                else
                                {
                                    cookedRecipeCalledIngredient.Name = cookedRecipeSubstituteIngredient.New;
                                    cookedRecipeCalledIngredient.CalledIngredient = null;
                                    cookedRecipeCalledIngredient.ProductStock = null;
                                }
                            }
                        }
                    }
                    break;
                case "add-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var addRecipeIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandAddRecipeIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var recipe = _context.Recipes.Include(r => r.CalledIngredients).FirstOrDefault(r => r.Name.ToLower() == addRecipeIngredient.Recipe.ToLower());
                        if (recipe == null)
                        {
                            systemResponse = "Error: Could not find recipe by name: " + addRecipeIngredient.Recipe;
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
                    }
                    break;
                case "add-cooked-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var addCookedRecipeIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandAddCookedRecipeIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var cookedRecipe = _context.CookedRecipes.Include(r => r.CookedRecipeCalledIngredients).OrderByDescending(cr => cr.Created).FirstOrDefault(r => r.Recipe.Name.ToLower() == addCookedRecipeIngredient.Recipe.ToLower());
                        if (cookedRecipe == null)
                        {
                            systemResponse = "Error: Could not find cooked recipe by name: " + addCookedRecipeIngredient.Recipe;
                        }
                        else
                        {
                            var cookedRecipeCalledIngredient = new CookedRecipeCalledIngredient
                            {
                                Name = addCookedRecipeIngredient.Name,
                                CookedRecipe = cookedRecipe,
                                Units = addCookedRecipeIngredient.Units,
                                UnitType = UnitTypeFromString(addCookedRecipeIngredient.UnitType)
                            };
                            cookedRecipe.CookedRecipeCalledIngredients.Add(cookedRecipeCalledIngredient);
                            _context.CookedRecipeCalledIngredients.Add(cookedRecipeCalledIngredient);
                        }
                    }
                    break;
                case "remove-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var deleteRecipeIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandDeleteRecipeIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var recipe = _context.Recipes.Include(r => r.CalledIngredients).FirstOrDefault(r => r.Name.ToLower() == deleteRecipeIngredient.Recipe.ToLower());
                        if (recipe == null)
                        {
                            systemResponse = "Error: Could not find recipe by name: " + deleteRecipeIngredient.Recipe;
                        }
                        else
                        {
                            var calledIngredient = recipe.CalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(deleteRecipeIngredient.Ingredient.ToLower()));

                            if (calledIngredient == null)
                            {
                                systemResponse = "Error: Could not find ingredient by name: " + deleteRecipeIngredient.Ingredient;
                            }
                            else
                            {
                                recipe.CalledIngredients.Remove(calledIngredient);
                            }
                        }
                    }
                    break;
                case "remove-cooked-recipe-ingredient":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var deleteCookedRecipeIngredient = JsonConvert.DeserializeObject<OpenApiChatCommandDeleteCookedRecipeIngredient>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var cookedRecipe = _context.CookedRecipes.Include(r => r.CookedRecipeCalledIngredients).FirstOrDefault(r => r.Recipe.Name.ToLower() == deleteCookedRecipeIngredient.Recipe.ToLower());
                        if (cookedRecipe == null)
                        {
                            systemResponse = "Error: Could not find cooked recipe by name: " + deleteCookedRecipeIngredient.Recipe;
                        }
                        else
                        {
                            var cookedRecipeCalledIngredient = cookedRecipe.CookedRecipeCalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(deleteCookedRecipeIngredient.Ingredient.ToLower()));

                            if (cookedRecipeCalledIngredient == null)
                            {
                                systemResponse = "Error: Could not find ingredient by name: " + deleteCookedRecipeIngredient.Ingredient;
                            }
                            else
                            {
                                cookedRecipe.CookedRecipeCalledIngredients.Remove(cookedRecipeCalledIngredient);
                            }
                        }
                    }
                    break;
                case "edit-recipe-ingredient-unittype":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var editRecipeIngredientUnitType = JsonConvert.DeserializeObject<OpenApiChatCommandEditRecipeIngredientUnitType>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var recipe = _context.Recipes.Include(r => r.CalledIngredients).FirstOrDefault(r => r.Name.ToLower().Contains(editRecipeIngredientUnitType.Recipe.ToLower()));

                        if (recipe == null)
                        {
                            systemResponse = "Error: Could not find recipe by name: " + editRecipeIngredientUnitType.Recipe;
                        }
                        else
                        {
                            var calledIngredient = recipe.CalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(editRecipeIngredientUnitType.Name.ToLower()));
                            if (calledIngredient == null)
                            {
                                systemResponse = "Error: Could not find ingredient by name: " + editRecipeIngredientUnitType.Name;
                            }
                            else
                            {
                                calledIngredient.UnitType = UnitTypeFromString(editRecipeIngredientUnitType.UnitType);
                            }
                        }
                    }
                    break;
                case "edit-cooked-recipe-ingredient-unittype":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var editCookedRecipeIngredientUnitType = JsonConvert.DeserializeObject<OpenApiChatCommandEditCookedRecipeIngredientUnitType>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var cookedRecipe = _context.CookedRecipes.Include(r => r.CookedRecipeCalledIngredients).FirstOrDefault(r => r.Recipe.Name.ToLower().Contains(editCookedRecipeIngredientUnitType.Recipe.ToLower()));

                        if (cookedRecipe == null)
                        {
                            systemResponse = "Error: Could not find cooked recipe by name: " + editCookedRecipeIngredientUnitType.Recipe;
                        }
                        else
                        {
                            var calledIngredient = cookedRecipe.CookedRecipeCalledIngredients.FirstOrDefault(ci => ci.Name.ToLower().Contains(editCookedRecipeIngredientUnitType.Name.ToLower()));
                            if (calledIngredient == null)
                            {
                                systemResponse = "Error: Could not find ingredient by name: " + editCookedRecipeIngredientUnitType.Name;
                            }
                            else
                            {
                                calledIngredient.UnitType = UnitTypeFromString(editCookedRecipeIngredientUnitType.UnitType);
                            }
                        }
                    }
                    break;
                case "edit-product-unit-type":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var editProductUnitType = JsonConvert.DeserializeObject<OpenApiChatCommandEditProductUnitType>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var product = _context.Products.FirstOrDefault(p => p.Name.ToLower().Contains(editProductUnitType.Product.ToLower()));
                        if (product == null)
                        {
                            systemResponse = "Error: Could not find product by name: " + editProductUnitType.Product;
                        }
                        else
                        {
                            product.UnitType = UnitTypeFromString(editProductUnitType.UnitType);
                        }
                    }
                    break;
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
                            systemResponse = "Error: Could not find recipe by name: " + deleteRecipe.Name;
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
                case "create-cooked-recipe":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var createCookedRecipe = JsonConvert.DeserializeObject<OpenApiChatCommandCreateCookedRecipe>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        var recipe = _context.Recipes
                            .Where(r => r.Name.ToLower() == createCookedRecipe.Name.ToLower()).Include(r => r.CalledIngredients).ThenInclude(ci => ci.ProductStock)
                            .SingleOrDefaultAsync(cancellationToken).Result;

                        if (recipe == null)
                        {
                            systemResponse = "Error: Could not find recipe by name: " + createCookedRecipe.Name;
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
                case "go-to-page":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var goToPage = JsonConvert.DeserializeObject<OpenApiChatCommandGoToPage>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));
                        notification.ChatCommand.NavigateToPage = string.Join('-', goToPage.Page.Split(' '));
                    }
                    break;
                case "none":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var none = JsonConvert.DeserializeObject<OpenApiChatCommandNone>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));
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
        }
        catch (Exception ex)
        {
            notification.ChatCommand.Error = FlattenException(ex);
            notification.ChatCommand.SystemResponse = "Error: " + ex.Message;
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
