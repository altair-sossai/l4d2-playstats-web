using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Models;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/game-info")]
[ApiController]
public class GameInfoController : ControllerBase
{
    private static readonly GameInfo.GameInfo GameInfo = L4D2PlayStats.GameInfo.GameInfo.Current;

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(GameInfo);
    }

    [HttpGet]
    [Route("configuration")]
    public IActionResult Configuration()
    {
        return Ok(GameInfo.Configuration);
    }

    [HttpPut("configuration")]
    [RequiredSecretKey]
    public IActionResult Configuration([FromBody] Configuration configuration)
    {
        GameInfo.Configuration = configuration;

        return Ok();
    }

    [HttpGet]
    [Route("round")]
    public IActionResult Round()
    {
        return Ok(GameInfo.Round);
    }

    [HttpPut("round")]
    [RequiredSecretKey]
    public IActionResult Round([FromBody] Round round)
    {
        GameInfo.Round = round;

        return Ok();
    }

    [HttpGet]
    [Route("scoreboard")]
    public IActionResult Scoreboard()
    {
        return Ok(GameInfo.Scoreboard);
    }

    [HttpPut("scoreboard")]
    [RequiredSecretKey]
    public IActionResult Scoreboard([FromBody] Scoreboard scoreboard)
    {
        GameInfo.Scoreboard = scoreboard;

        return Ok();
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

    [HttpPut("players")]
    [RequiredSecretKey]
    public IActionResult UpdatePlayers([FromBody] PlayersCommand command)
    {
        GameInfo.Survivors = command.Survivors?.ToArray() ?? [];
        GameInfo.Infecteds = command.Infecteds?.ToArray() ?? [];
        GameInfo.Spectators = command.Spectators?.ToArray() ?? [];

        return Ok();
    }

    [HttpGet]
    [Route("messages")]
    public IActionResult Messages(int after = 0)
    {
        return Ok(GameInfo.Messages.Where(m => m.When > after));
    }

    [HttpPut("messages")]
    [RequiredSecretKey]
    public IActionResult AddMessage([FromBody] ChatMessageCommand command)
    {
        GameInfo.AddMessage(command);

        return Ok();
    }
}