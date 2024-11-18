using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/cache")]
[ApiController]
public class CacheController(IMemoryCache memoryCache) : ControllerBase
{
    private static DateTime _lastClear = DateTime.MinValue;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);

    [HttpPost("clear")]
    public IActionResult Clear()
    {
        if (DateTime.UtcNow - _lastClear < CacheDuration)
            return BadRequest("Cache was cleared recently");

        _lastClear = DateTime.UtcNow;

        memoryCache.Remove("Matches");
        memoryCache.Remove("Ranking");

        return Ok();
    }
}