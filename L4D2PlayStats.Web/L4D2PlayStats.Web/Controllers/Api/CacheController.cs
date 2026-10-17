using L4D2PlayStats.Core.Cache.Services;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/cache")]
[ApiController]
public class CacheController(ICacheService cacheService) : ControllerBase
{
    [HttpPost("clear")]
    [RequiredSecretKey]
    public IActionResult Clear()
    {
        cacheService.ClearCache();

        return Ok();
    }
}