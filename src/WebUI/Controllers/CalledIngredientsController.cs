using WOF.Application.CalledIngredients.Commands.CreateCalledIngredient;
using WOF.Application.CalledIngredients.Commands.DeleteCalledIngredient;
using WOF.Application.CalledIngredients.Commands.UpdateCalledIngredient;
using WOF.Application.CalledIngredients.Commands.UpdateCalledIngredientDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.CalledIngredients.Queries.GetCalledIngredients;
using WOF.Application.CalledIngredients;

namespace WOF.WebUI.Controllers;

[Authorize]
public class CalledIngredientsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<GetCalledIngredientsVm>> GetCalledIngredients()
    {
        return await Mediator.Send(new GetCalledIngredientsQuery());
    }

    [HttpGet("GetCalledIngredientDetails/{id}")]
    public async Task<ActionResult<CalledIngredientDetailsVm>> GetCalledIngredientDetails(int id)
    {
        return await Mediator.Send(new GetCalledIngredientDetailsQuery
        {
            Id = id
        });
    }

    [HttpGet("SearchProductStockName")]
    public async Task<ActionResult<CalledIngredientDetailsVm>> SearchProductStockName([FromQuery] SearchProductStockNameQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCalledIngredientCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCalledIngredientCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<ActionResult<CalledIngredientDetailsVm>> UpdateCalledIngredientDetails(int id, UpdateCalledIngredientDetailsCommand command)
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
        await Mediator.Send(new DeleteCalledIngredientCommand(id));

        return NoContent();
    }
}
