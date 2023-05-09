using WOF.Application.Recipes.Commands.CreateRecipes;
using WOF.Application.Recipes.Commands.DeleteRecipes;
using WOF.Application.Recipes.Commands.UpdateRecipes;
using WOF.Application.Recipes.Queries.GetRecipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Domain.Entities;
using WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

namespace WOF.WebUI.Controllers;

[Authorize]
public class RecipesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<RecipesVm>> Get()
    {
        return await Mediator.Send(new GetRecipesQuery());
    }

    [HttpPost]
    public async Task<ActionResult<RecipeDto>> Create(CreateRecipeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("UpdateName/{id}")]
    public async Task<ActionResult<RecipeDto>> UpdateName(int id, UpdateRecipeNameCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpPut("UpdateServes/{id}")]
    public async Task<ActionResult<RecipeDto>> UpdateServes(int id, UpdateRecipeServesCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteRecipeCommand(id));

        return NoContent();
    }
}
