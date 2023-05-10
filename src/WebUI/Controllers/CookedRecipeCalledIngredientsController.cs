using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.CookedRecipeCalledIngredients;
using WOF.Application.CookedRecipeCalledIngredients.Commands.CreateCookedRecipeCalledIngredient;
using WOF.Application.CookedRecipeCalledIngredients.Commands.DeleteCookedRecipeCalledIngredient;
using WOF.Application.CookedRecipeCalledIngredients.Commands.UpdateCookedRecipeCalledIngredient;
using WOF.Application.CookedRecipeCalledIngredients.Commands.UpdateCookedRecipeCalledIngredientDetails;
using WOF.Application.CookedRecipeCalledIngredients.Queries.GetCookedRecipeCalledIngredients;

namespace WOF.WebUI.Controllers;

[Authorize]
public class CookedRecipeCalledIngredientsController : ApiControllerBase
{
    [HttpGet("GetCookedRecipeCalledIngredientDetails/{id}")]
    public async Task<ActionResult<CookedRecipeCalledIngredientDetailsVm>> GetCookedRecipeCalledIngredientDetails(int id)
    {
        return await Mediator.Send(new GetCookedRecipeCalledIngredientDetailsQuery
        {
            Id = id
        });
    }

    [HttpGet("SearchProductStockName")]
    public async Task<ActionResult<CookedRecipeCalledIngredientDetailsVm>> SearchProductStockName([FromQuery] SearchProductStockNameQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCookedRecipeCalledIngredientCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCookedRecipeCalledIngredientCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<CookedRecipeCalledIngredientDetailsVm>> UpdateCookedRecipeCalledIngredientDetails(int id, UpdateCookedRecipeCalledIngredientDetailsCommand command)
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
        await Mediator.Send(new DeleteCookedRecipeCalledIngredientCommand(id));

        return NoContent();
    }
}
