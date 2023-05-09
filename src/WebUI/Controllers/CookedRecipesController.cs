using WOF.Application.CookedRecipes.Commands.CreateCookedRecipes;
using WOF.Application.CookedRecipes.Commands.DeleteCookedRecipes;
using WOF.Application.CookedRecipes.Queries.GetCookedRecipes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WOF.WebUI.Controllers;

[Authorize]
public class CookedRecipesController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CookedRecipesVm>> Get()
    {
        return await Mediator.Send(new GetCookedRecipesQuery());
    }

    [HttpPost]
    public async Task<ActionResult<CookedRecipeDto>> Create(CreateCookedRecipeCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteCookedRecipeCommand(id));

        return NoContent();
    }
}
