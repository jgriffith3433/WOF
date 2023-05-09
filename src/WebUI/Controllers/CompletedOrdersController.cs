using WOF.Application.CompletedOrders.Commands.CreateCompletedOrders;
using WOF.Application.CompletedOrders.Commands.DeleteCompletedOrders;
using WOF.Application.CompletedOrders.Commands.UpdateCompletedOrders;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrderProducts;
using WOF.Application.CompletedOrders.Commands.CreateCompletedOrderProduct;
using WOF.Application.CompletedOrders.Commands.DeleteCompletedOrderProduct;
using WOF.Application.CompletedOrders.Commands.UpdateCompletedOrderProduct;

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
    public async Task<ActionResult<CompletedOrderDto>> Get(int id)
    {
        return await Mediator.Send(new GetCompletedOrderQuery
        {
            Id = id
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
        if (command == null || id != command.Id)
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

    //Completed Order Products

    [HttpGet("GetCompletedOrderProduct/{id}")]
    public async Task<ActionResult<CompletedOrderProductDto>> GetCompletedOrderProduct(int id)
    {
        return await Mediator.Send(new GetCompletedOrderProductQuery
        {
            Id = id
        });
    }

    [HttpGet("SearchCompletedOrderProductName")]
    public async Task<ActionResult<CompletedOrderProductDto>> SearchCompletedOrderProductName([FromQuery] SearchCompletedOrderProductNameQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost("CreateCompletedOrderProduct")]
    public async Task<ActionResult<int>> CreateCompletedOrderProduct(CreateCompletedOrderProductCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("UpdateCompletedOrderProduct/{id}")]
    public async Task<ActionResult<CompletedOrderProductDto>> UpdateCompletedOrderProduct(int id, UpdateCompletedOrderProductCommand command)
    {
        if (command == null || id != command.Id)
        {
            return BadRequest();
        }

        return await Mediator.Send(command);
    }

    [HttpDelete("DeleteCompletedOrderProduct/{id}")]
    public async Task<ActionResult> DeleteCompletedOrderProduct(int id)
    {
        await Mediator.Send(new DeleteCompletedOrderProductCommand(id));

        return NoContent();
    }
}
