﻿using WOF.Application.Common.Models;
using WOF.Application.Ingredients.Commands.CreateIngredient;
using WOF.Application.Ingredients.Commands.DeleteIngredient;
using WOF.Application.Ingredients.Commands.UpdateIngredient;
using WOF.Application.Ingredients.Commands.UpdateIngredientDetail;
using WOF.Application.Ingredients.Queries.GetIngredientsWithPagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WOF.WebUI.Controllers;

[Authorize]
public class IngredientsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<PaginatedList<IngredientBriefDto>>> GetIngredientsWithPagination([FromQuery] GetIngredientsWithPaginationQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateIngredientCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateIngredientCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }

    [HttpPut("[action]")]
    public async Task<ActionResult> UpdateIngredientDetails(int id, UpdateIngredientDetailCommand command)
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
        await Mediator.Send(new DeleteIngredientCommand(id));

        return NoContent();
    }
}