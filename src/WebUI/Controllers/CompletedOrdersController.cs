using WOF.Application.CompletedOrders.Commands.CreateCompletedOrders;
using WOF.Application.CompletedOrders.Commands.DeleteCompletedOrders;
using WOF.Application.CompletedOrders.Commands.UpdateCompletedOrders;
//using WOF.Application.CompletedOrders.Queries.ExportCompletedOrders;
using WOF.Application.CompletedOrders.Queries.GetCompletedOrders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace WOF.WebUI.Controllers;

[Authorize]
public class CompletedOrdersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<CompletedOrdersVm>> Get()
    {
        return await Mediator.Send(new GetCompletedOrdersQuery());
    }

    //[HttpGet("{id}")]
    //public async Task<FileResult> Get(int id)
    //{
    //    var vm = await Mediator.Send(new ExportTodosQuery { ListId = id });

    //    return File(vm.Content, vm.ContentType, vm.FileName);
    //}

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
