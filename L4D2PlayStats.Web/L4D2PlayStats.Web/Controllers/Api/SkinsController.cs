using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/skins")]
[ApiController]
public class SkinsController(ISkinService skinService) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var models = skinService
            .GetAllSkins()
            .Select(skin => new SkinLevelModel(skin))
            .ToList();

        return Ok(models);
    }
}