using L4D2PlayStats.GameInfo.Commands;
using L4D2PlayStats.GameInfo.Models;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/game-info")]
[ApiController]
public class GameInfoController(IUserAvatar userAvatar) : ControllerBase
{
    private readonly GameInfo.GameInfo _gameInfo = GameInfo.GameInfo.GetOrInitializeInstance(userAvatar);

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(_gameInfo);
    }

    [HttpGet]
    [Route("configuration")]
    public IActionResult Configuration()
    {
        return Ok(_gameInfo.Configuration);
    }

    [HttpPut("configuration")]
    [RequiredSecretKey]
    public IActionResult Configuration([FromBody] Configuration configuration)
    {
        _gameInfo.Configuration = configuration;

        return Ok();
    }

    [HttpGet]
    [Route("round")]
    public IActionResult Round()
    {
        return Ok(_gameInfo.Round);
    }

    [HttpPut("round")]
    [RequiredSecretKey]
    public IActionResult Round([FromBody] Round round)
    {
        _gameInfo.Round = round;

        return Ok();
    }

    [HttpGet]
    [Route("scoreboard")]
    public IActionResult Scoreboard()
    {
        return Ok(_gameInfo.Scoreboard);
    }

    [HttpPut("scoreboard")]
    [RequiredSecretKey]
    public IActionResult Scoreboard([FromBody] Scoreboard scoreboard)
    {
        _gameInfo.Scoreboard = scoreboard;

        return Ok();
    }

    [HttpGet]
    [Route("players")]
    public IActionResult Players()
    {
        return Ok(new
        {
            _gameInfo.Survivors,
            _gameInfo.Infecteds,
            _gameInfo.Spectators
        });
    }

    [HttpPut("players")]
    [RequiredSecretKey]
    public IActionResult UpdatePlayers([FromBody] PlayersCommand command)
    {
        _gameInfo.Survivors = command.Survivors?.ToArray() ?? [];
        _gameInfo.Infecteds = command.Infecteds?.ToArray() ?? [];
        _gameInfo.Spectators = command.Spectators?.ToArray() ?? [];

        return Ok();
    }

    [HttpGet]
    [Route("messages")]
    public IActionResult Messages(int after = 0)
    {
        return Ok(_gameInfo.Messages.Where(m => m.When > after));
    }

    [HttpPut("messages")]
    [RequiredSecretKey]
    public IActionResult AddMessage([FromBody] ChatMessageCommand command)
    {
        _gameInfo.AddMessage(command);

        return Ok();
    }
}