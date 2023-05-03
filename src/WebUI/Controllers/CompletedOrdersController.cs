using WOF.Application.CompletedOrders.Commands.CreateCompletedOrders;
using WOF.Application.CompletedOrders.Commands.DeleteCompletedOrders;
using WOF.Application.CompletedOrders.Commands.UpdateCompletedOrders;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Domain.Entities;
using WOF.Application.Walmart.Commands;
using WOF.Application.Ingredients.Queries.GetIngredients;

namespace WOF.WebUI.Controllers;

[Authorize]
public class CompletedOrdersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CompletedOrdersVm>> Get()
    {
        return await Mediator.Send(new GetCompletedOrdersQuery());
    }

    [HttpGet("{id}")]
    public async Task<CompletedOrderDto> Get(int id)
    {
        return await Mediator.Send(new CreateIngredientsFromCompletedOrderCommand
        {
            CompletedOrderId = id
        });
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateCompletedOrderCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateCompletedOrderCommand command)
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
        await Mediator.Send(new DeleteCompletedOrderCommand(id));

        return NoContent();
    }
}
