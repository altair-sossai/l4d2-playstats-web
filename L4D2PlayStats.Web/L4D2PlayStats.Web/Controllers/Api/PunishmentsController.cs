using L4D2PlayStats.Core.Cache.Services;
using L4D2PlayStats.Sdk.Punishments;
using L4D2PlayStats.Sdk.Punishments.Commands;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/punishments")]
[Authorize]
[ApiController]
public class PunishmentsController(IPunishmentsService punishmentsService, ICacheService cacheService) : ControllerBase
{
    [HttpPost]
    [AdminAuthorize]
    public async Task<IActionResult> PostAsync([FromBody] PunishmentCommand command, CancellationToken cancellationToken)
    {
        var result = await punishmentsService.PostAsync(command, cancellationToken);

        cacheService.ClearCache();

        return Ok(result);
    }

    [HttpDelete("{communityId}")]
    [AdminAuthorize]
    public async Task<IActionResult> DeleteAsync(string communityId, CancellationToken cancellationToken)
    {
        await punishmentsService.DeleteAsync(communityId, cancellationToken);

        cacheService.ClearCache();

        return NoContent();
    }
}