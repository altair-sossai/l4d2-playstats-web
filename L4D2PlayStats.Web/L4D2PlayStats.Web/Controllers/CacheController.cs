using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace L4D2PlayStats.Web.Controllers;

[Route("api/cache")]
[ApiController]
public class CacheController(IMemoryCache memoryCache) : ControllerBase
{
    [HttpPost("clear")]
    public IActionResult Clear()
    {
        memoryCache.Remove("Matches");
        memoryCache.Remove("Ranking");

        return Ok();
    }
}