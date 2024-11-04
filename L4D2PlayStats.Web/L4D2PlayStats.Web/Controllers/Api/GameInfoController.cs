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

    [HttpGet]
    [Route("players")]
    public IActionResult Players()
    {
        return Ok(new
        {
            GameInfo.Survivors,
            GameInfo.Infecteds,
            GameInfo.Spectators
        });
    }

    [HttpGet]
    [Route("messages")]
    public IActionResult Messages()
    {
        return Ok(GameInfo.Messages);
    }

    [HttpPut]
    [RequiredSecretKey]
    public IActionResult Put([FromBody] GameInfoCommand command)
    {
        GameInfo.Update(command);

        return Ok();
    }

    [HttpPut("players")]
    [RequiredSecretKey]
    public IActionResult UpdatePlayers([FromBody] PlayersCommand command)
    {
        GameInfo.UpdatePlayers(command);

        return Ok();
    }

    [HttpPut("messages")]
    [RequiredSecretKey]
    public IActionResult AddMessage([FromBody] ChatMessageCommand command)
    {
        GameInfo.AddMessage(command);

        return Ok();
    }
}