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
                case "edit-recipe-ingredient-unittype":
                case "edit-ingredient-unittype":
                    {
                        var startIndex = notification.ChatCommand.RawReponse.IndexOf('{');
                        var endIndex = notification.ChatCommand.RawReponse.LastIndexOf('}');
                        var editIngredientUnitType = JsonConvert.DeserializeObject<OpenApiChatCommandEditIngredientUnitType>(notification.ChatCommand.RawReponse.Substring(startIndex, endIndex - startIndex + 1));

                        //ingredients can be CalledIngredients, CookedRecipeCalledIngredients, and Products
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
                                        calledIngredient.SizeType = SizeTypeFromString(editIngredientUnitType.UnitType);
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
                                        cookedRecipeCalledIngredient.SizeType = SizeTypeFromString(editIngredientUnitType.UnitType);
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
                                        product.SizeType = SizeTypeFromString(editIngredientUnitType.UnitType);
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
                                SizeType = SizeTypeFromString(createRecipeIngredient.UnitType)
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

    private SizeType SizeTypeFromString(string sizeTypeStr)
    {
        switch (sizeTypeStr.ToLower())
        {
            case "":
            case "none":
                return SizeType.None;
            case "bulk":
                return SizeType.Bulk;
            case "ounce":
            case "ounces":
                return SizeType.Ounce;
            case "teaspoon":
            case "teaspoons":
                return SizeType.Teaspoon;
            case "tablespoon":
            case "tablespoons":
                return SizeType.Tablespoon;
            case "pound":
            case "pounds":
                return SizeType.Pound;
            case "cup":
            case "cups":
                return SizeType.Cup;
            case "clove":
            case "cloves":
                return SizeType.Cloves;
            case "can":
            case "cans":
                return SizeType.Can;
            case "whole":
            case "wholes":
                return SizeType.Whole;
            case "package":
            case "packages":
                return SizeType.Package;
            case "bar":
            case "bars":
                return SizeType.Bar;
            case "bun":
            case "buns":
                return SizeType.Bun;
            default:
                return SizeType.None;
        }
    }
}
