using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Extensions;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.UserAvatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/external-chat")]
[ApiController]
public class ExternalChatController(ICurrentUser currentUser, IUserAvatar userAvatar) : ControllerBase
{
    private readonly GameInfo _gameInfo = GameInfo.GetOrInitializeInstance(userAvatar);

    [HttpGet]
    public IActionResult GetAsync([FromQuery] long after = 0)
    {
        var messages = _gameInfo.ExternalMessages.After(after);

        return Ok(messages);
    }

    [HttpPost]
    [Authorize]
    public IActionResult PostAsync([FromBody] ExternalChatMessageCommand command)
    {
        var user = new User(currentUser);

        if (command.IsCommandMessage
            && currentUser.IsAdmin
            && ServerCommand.TryParse(command.Message, out var serverCommand)
            && serverCommand != null)
            _gameInfo.ServerCommands.Enqueue(serverCommand);

        var message = _gameInfo.AddExternalMessage(user, command);

        return Ok(message);
    }
}