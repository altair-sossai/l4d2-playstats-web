using L4D2PlayStats.Ranking;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class PatentController(IRankingServiceCached rankingServiceCached) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.Patent = "active";

        var config = await rankingServiceCached.ExperienceConfigAsync();

        return View(config);
    }
}