using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/game-info")]
[ApiController]
public class GameInfoController : ControllerBase
{
    private static readonly GameInfo.GameInfo GameInfo = new();

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(GameInfo);
    }

    [HttpPut]
    [RequiredSecretKey]
    public IActionResult Put([FromBody] GameInfoCommand command)
    {
        GameInfo.Update(command);

        return Ok();
    }
}