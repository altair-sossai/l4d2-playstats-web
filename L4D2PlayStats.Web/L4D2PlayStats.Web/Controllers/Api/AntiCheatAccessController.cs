using L4D2PlayStats.Core.AntiCheatAccess;
using L4D2PlayStats.Core.AntiCheatAccess.Commands;
using L4D2PlayStats.Core.AntiCheatAccess.Enums;
using L4D2PlayStats.Core.AntiCheatAccess.Services;
using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/anti-cheat-access")]
[ApiController]
public class AntiCheatAccessController(IAntiCheatAccessService accessService, ICurrentUser currentUser) : ControllerBase
{
    private const string GameAdminName = "Console";

    [HttpGet]
    [Authorize]
    [AdminAuthorize]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        return Ok(await accessService.GetAsync(cancellationToken: cancellationToken));
    }

    [HttpPut("mode")]
    [Authorize]
    [AdminAuthorize]
    public async Task<IActionResult> PutModeAsync([FromBody] AntiCheatAccessModeCommand command, CancellationToken cancellationToken)
    {
        if (!AntiCheatAccessNames.TryParseMode(command.Mode, out var mode))
            return BadRequest(new { errorMessage = "The mode is invalid." });

        await accessService.SetModeAsync(mode, new User(currentUser), AntiCheatAccessSource.Site, cancellationToken);

        return Ok(await accessService.GetAsync(cancellationToken: cancellationToken));
    }

    [HttpPost("{list}")]
    [Authorize]
    [AdminAuthorize]
    public async Task<IActionResult> PostAsync(string list, [FromBody] AntiCheatAccessPlayerCommand command, CancellationToken cancellationToken)
    {
        if (!AntiCheatAccessNames.TryParseList(list, out var accessList))
            return NotFound();

        try
        {
            var player = await accessService.AddAsync(accessList, command.Player, command.Name, new User(currentUser), AntiCheatAccessSource.Site, cancellationToken);

            return Ok(player);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { errorMessage = exception.Message });
        }
    }

    [HttpDelete("{list}/{communityId}")]
    [Authorize]
    [AdminAuthorize]
    public async Task<IActionResult> DeleteAsync(string list, string communityId, CancellationToken cancellationToken)
    {
        if (!AntiCheatAccessNames.TryParseList(list, out var accessList))
            return NotFound();

        await accessService.RemoveAsync(accessList, communityId, AntiCheatAccessSource.Site, cancellationToken);

        return NoContent();
    }

    [HttpGet("server")]
    [RequiredSecretKey]
    public async Task<IActionResult> GetServerAsync(CancellationToken cancellationToken)
    {
        return Ok(await accessService.GetServerAsync(cancellationToken));
    }

    [HttpPut("server/mode")]
    [RequiredSecretKey]
    public async Task<IActionResult> PutServerModeAsync([FromBody] AntiCheatAccessModeCommand command, CancellationToken cancellationToken)
    {
        if (!AntiCheatAccessNames.TryParseMode(command.Mode, out var mode))
            return BadRequest(new { errorMessage = "The mode is invalid." });

        await accessService.SetModeAsync(mode, GameAdmin(command.AdminCommunityId, command.AdminName), AntiCheatAccessSource.Game, cancellationToken);

        return Ok(await accessService.GetServerAsync(cancellationToken));
    }

    [HttpPost("server/{list}")]
    [RequiredSecretKey]
    public async Task<IActionResult> PostServerAsync(string list, [FromBody] AntiCheatAccessPlayerCommand command, CancellationToken cancellationToken)
    {
        if (!AntiCheatAccessNames.TryParseList(list, out var accessList))
            return NotFound();

        try
        {
            await accessService.AddAsync(accessList, command.Player, command.Name, GameAdmin(command.AdminCommunityId, command.AdminName), AntiCheatAccessSource.Game, cancellationToken);

            return Ok(await accessService.GetServerAsync(cancellationToken));
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new { errorMessage = exception.Message });
        }
    }

    [HttpDelete("server/{list}")]
    [RequiredSecretKey]
    public async Task<IActionResult> DeleteServerAsync(string list, [FromQuery] string? player, CancellationToken cancellationToken)
    {
        if (!AntiCheatAccessNames.TryParseList(list, out var accessList))
            return NotFound();

        if (!SteamIdentifiers.TryParse(player, out _))
            return BadRequest(new { errorMessage = "The Steam ID is invalid." });

        await accessService.RemoveAsync(accessList, player, AntiCheatAccessSource.Game, cancellationToken);

        return Ok(await accessService.GetServerAsync(cancellationToken));
    }

    private static User GameAdmin(string? communityId, string? name)
    {
        var parsed = SteamIdentifiers.TryParse(communityId, out var identifiers) ? identifiers.CommunityId?.ToString() : null;

        return new User
        {
            CommunityId = parsed,
            Name = string.IsNullOrWhiteSpace(name) ? GameAdminName : name.Trim()
        };
    }
}
