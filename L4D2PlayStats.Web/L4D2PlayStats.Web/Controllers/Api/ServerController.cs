using L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[ApiController]
public class ServerController(IServerInfoServiceCached serverInfoService) : ControllerBase
{
    [HttpGet("api/server")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var server = await serverInfoService.GetServerInfoAsync(cancellationToken);

        return Ok(server);
    }

    [HttpGet("api/servers")]
    public IActionResult GetLegacy()
    {
        return RedirectPermanent("/api/server");
    }
}