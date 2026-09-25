using L4D2PlayStats.Core;
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

        var latest = await antiCheatServiceCached.GetLatestVersionAsync(cancellationToken);

        var model = new AntiCheatModel(latest, AppConsts.AntiCheatDownloadUrl);

        return View(model);
    }
}