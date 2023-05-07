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

    [HttpPut("{id}")]
    public async Task<ActionResult<RecipeDto>> Update(int id, UpdateRecipeCommand command)
    {
        if (id != command.Id)
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
