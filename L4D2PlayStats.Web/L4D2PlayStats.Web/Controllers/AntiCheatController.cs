using L4D2PlayStats.Core;
using L4D2PlayStats.Core.AntiCheat.Services.Cache;
using L4D2PlayStats.Core.AntiCheatAccess.Enums;
using L4D2PlayStats.Core.AntiCheatAccess.Results;
using L4D2PlayStats.Core.AntiCheatAccess.Services;
using L4D2PlayStats.Core.Auth;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace L4D2PlayStats.Web.Controllers;

public class AntiCheatController(IAntiCheatServiceCached antiCheatServiceCached, IAntiCheatAccessService accessService, ICurrentUser currentUser) : Controller
{
    [Route("anti-cheat")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        ViewBag.AntiCheat = "active";

        var latest = await antiCheatServiceCached.GetLatestVersionAsync(cancellationToken);
        var access = await AccessAsync(cancellationToken);

        var model = new AntiCheatModel(latest, AppConsts.AntiCheatDownloadUrl)
        {
            CurrentUserRequired = access == null ? null : IsCurrentUserRequired(access)
        };

        return View(model);
    }

    private async Task<AntiCheatAccessResult?> AccessAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await accessService.GetAsync(false, cancellationToken);
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            Log.Warning(exception, "Could not read the anti-cheat access settings");

            return null;
        }
    }

    private bool? IsCurrentUserRequired(AntiCheatAccessResult access)
    {
        if (!currentUser.IsAuthenticated || string.IsNullOrEmpty(currentUser.CommunityId))
            return null;

        var communityId = currentUser.CommunityId;

        return access.Mode == AntiCheatAccessMode.Everyone
            ? access.Whitelist.All(player => player.CommunityId != communityId)
            : access.Required.Any(player => player.CommunityId == communityId);
    }
}
