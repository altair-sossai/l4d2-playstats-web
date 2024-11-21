using L4D2PlayStats.Cache.Services;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/cache")]
[ApiController]
public class CacheController(ICacheService cacheService) : ControllerBase
{
    [HttpPost("clear")]
    public IActionResult Clear()
    {
        cacheService.ClearCache();

        return Ok();
    }
}