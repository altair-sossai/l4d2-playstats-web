using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.Skins.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class PatentsController(IRankingServiceCached rankingServiceCached, IPatentService patentService, ISkinService skinService) : Controller
{
    [Route("patents")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.Patent = "active";

        var config = await rankingServiceCached.ExperienceConfigAsync(cancellationToken);
        var patents = patentService.GetAllPatents();
        var skins = skinService.GetAllSkins();

        var model = new PatentModel(config, patents, skins);

        return View(model);
    }
}