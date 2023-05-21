using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using WOF.Application.Chat.Queries.GetResponse;

namespace WOF.WebUI.Controllers;

[Authorize]
public class ChatController : ApiControllerBase
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ChatController(IWebHostEnvironment env)
    {
        _webHostEnvironment = env;
    }

    [HttpPost]
    public async Task<ActionResult<GetChatResponseVm>> Create(GetChatResponseQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost("speech")]
    public async Task<ActionResult<GetChatTextFromSpeechVm>> Speech([FromForm] IFormFile speech)
    {
        var uploads = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

        if (!Path.Exists(uploads))
        {
            Directory.CreateDirectory(uploads);
        }

        var filePath = Path.Combine(uploads, speech.FileName);
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await speech.CopyToAsync(fileStream);
        }

        return await Mediator.Send(new GetChatTextFromSpeechQuery
        {
            Speech = ConvertToByteArrayContent(speech)
        });
    }


    private byte[] ConvertToByteArrayContent(IFormFile audofile)
    {
        byte[] data;

        using (var br = new BinaryReader(audofile.OpenReadStream()))
        {
            data = br.ReadBytes((int)audofile.OpenReadStream().Length);
        }
        return data;
    }
}
