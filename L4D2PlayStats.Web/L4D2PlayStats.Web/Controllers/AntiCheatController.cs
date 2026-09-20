using L4D2PlayStats.Core.AntiCheat;
using L4D2PlayStats.Core.AntiCheat.Services.Cache;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class AntiCheatController(IAntiCheatServiceCached antiCheatServiceCached) : Controller
{
    [Route("anti-cheat")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.AntiCheat = "active";

        var version = await antiCheatServiceCached.GetLatestVersionAsync(cancellationToken);

        var model = new AntiCheatModel(version, AntiCheatUrls.Download);

        return View(model);
    }
}