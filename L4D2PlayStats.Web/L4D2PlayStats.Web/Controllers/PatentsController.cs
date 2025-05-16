using L4D2PlayStats.Core.Patents.Services;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class PatentsController(IRankingServiceCached rankingServiceCached, IPatentService patentService) : Controller
{
    [Route("patents")]
    public async Task<IActionResult> Index()
    {
        ViewBag.Patent = "active";

        var config = await rankingServiceCached.ExperienceConfigAsync();
        var patents = patentService.GetAllPatents();

        var model = new PatentModel(config, patents);

        return View(model);
    }
}