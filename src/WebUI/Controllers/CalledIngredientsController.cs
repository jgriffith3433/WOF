using WOF.Application.Common.Models;
using WOF.Application.CalledIngredients.Commands.CreateCalledIngredient;
using WOF.Application.CalledIngredients.Commands.DeleteCalledIngredient;
using WOF.Application.CalledIngredients.Commands.UpdateCalledIngredient;
using WOF.Application.CalledIngredients.Commands.UpdateCalledIngredientDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.CalledIngredients.Queries;
using WOF.Application.CalledIngredients.Queries.GetCalledIngredients;

namespace WOF.WebUI.Controllers;

[Authorize]
public class CalledIngredientsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetCalledIngredientsVm>> GetCalledIngredients()
    {
        return await Mediator.Send(new GetCalledIngredientsQuery());
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCalledIngredientCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCalledIngredientCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<ActionResult> UpdateCalledIngredientDetails(int id, UpdateCalledIngredientDetailCommand command)
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
        await Mediator.Send(new DeleteCalledIngredientCommand(id));

        return NoContent();
    }
}
