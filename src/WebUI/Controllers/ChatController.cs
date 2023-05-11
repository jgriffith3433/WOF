using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WOF.Application.Chat.Queries.GetResponse;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ChatController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<GetChatResponseVm>> Create(GetChatResponseQuery query)
    {
        return await Mediator.Send(query);
    }
}
