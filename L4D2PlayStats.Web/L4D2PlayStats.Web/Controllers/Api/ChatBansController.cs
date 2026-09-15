using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.ChatBans.Commands;
using L4D2PlayStats.Core.ChatBans.Services;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/chat-bans")]
[Authorize]
[ApiController]
public class ChatBansController(IChatBanService chatBanService, ICurrentUser currentUser) : ControllerBase
{
    [HttpGet]
    [AdminAuthorize]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var result = await chatBanService.GetAllAsync(cancellationToken);

        return Ok(result);
    }

    [HttpGet("recent-senders")]
    [AdminAuthorize]
    public async Task<IActionResult> GetRecentSendersAsync(CancellationToken cancellationToken)
    {
        var result = await chatBanService.GetRecentSendersAsync(cancellationToken);

        return Ok(result);
    }

    [HttpPost]
    [AdminAuthorize]
    public async Task<IActionResult> PostAsync([FromBody] ChatBanCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var admin = new User(currentUser);
            var result = await chatBanService.AddOrUpdateAsync(command, admin, cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { errorMessage = exception.Message });
        }
    }

    [HttpDelete("{communityId}")]
    [AdminAuthorize]
    public async Task<IActionResult> DeleteAsync(string communityId, CancellationToken cancellationToken)
    {
        await chatBanService.DeleteAsync(communityId, cancellationToken);

        return NoContent();
    }
}