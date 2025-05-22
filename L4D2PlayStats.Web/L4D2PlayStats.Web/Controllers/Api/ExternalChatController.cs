using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.ExternalChat.Commands;
using L4D2PlayStats.Core.ExternalChat.Models;
using L4D2PlayStats.Core.ExternalChat.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/external-chat")]
[ApiController]
public class ExternalChatController(ICurrentUser currentUser, IExternalChatService externalChatService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAsync([FromQuery] long after = 0)
    {
        var messages = externalChatService.GetMessages(after);

        return Ok(messages);
    }

    [HttpPost]
    [Authorize]
    public IActionResult PostAsync([FromBody] MessageCommand command)
    {
        var user = new User(currentUser);
        var message = externalChatService.SendMessage(user, command);

        return Ok(message);
    }
}