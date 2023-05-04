using WOF.Application.Recipes.Commands.CreateRecipes;
using WOF.Application.Recipes.Commands.DeleteRecipes;
using WOF.Application.Recipes.Commands.UpdateRecipes;
using WOF.Application.Recipes.Queries.GetRecipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Domain.Entities;
using WOF.Application.Walmart.Commands;
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

    [HttpGet("{id}")]
    public async Task<RecipeDto> Get(int id)
    {
        return await Mediator.Send(new CreateCalledIngredientsFromRecipeCommand
        {
            RecipeId = id
        });
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateRecipeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateRecipeCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteRecipeCommand(id));

        return NoContent();
    }
}
