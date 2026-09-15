using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.ChatBans.Services;
using L4D2PlayStats.Core.GameInfo;
using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Extensions;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Core.UserAvatar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/external-chat")]
[ApiController]
public class ExternalChatController(ICurrentUser currentUser, IUserAvatar userAvatar, IChatBanService chatBanService) : ControllerBase
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
    public async Task<IActionResult> PostAsync([FromBody] ExternalChatMessageCommand command, CancellationToken cancellationToken)
    {
        var chatBan = await chatBanService.FindAsync(currentUser.CommunityId, cancellationToken);

        if (chatBan != null)
        {
            var reason = string.IsNullOrWhiteSpace(chatBan.Reason) ? "You have been banned from the chat." : $"You have been banned from the chat. Reason: {chatBan.Reason}";

            return StatusCode(StatusCodes.Status403Forbidden, SendExternalMessageResult.FailureResult(reason));
        }

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