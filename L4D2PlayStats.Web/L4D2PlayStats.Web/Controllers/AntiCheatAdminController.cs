using L4D2PlayStats.Core.AntiCheatAccess;
using L4D2PlayStats.Core.AntiCheatAccess.Results;
using L4D2PlayStats.Core.AntiCheatAccess.Services;
using L4D2PlayStats.Web.Attributes;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Authorize]
[AdminAuthorize]
public class AntiCheatAdminController(IAntiCheatAccessService accessService) : Controller
{
    private const int PageSize = 20;

    [Route("anti-cheat-admin")]
    public async Task<IActionResult> Index(string? list, string? q, int page = 1, CancellationToken cancellationToken = default)
    {
        ViewBag.Admin = "active";
        ViewBag.AntiCheatAdmin = "active";

        var access = await accessService.GetAsync(cancellationToken: cancellationToken);
        var shownList = AntiCheatAccessNames.TryParseList(list, out var parsed) ? parsed : access.Mode.ListOf();
        var term = q?.Trim();

        var filtered = access[shownList]
            .Where(player => string.IsNullOrEmpty(term)
                             || player.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                             || player.CommunityId.Contains(term, StringComparison.OrdinalIgnoreCase)
                             || (player.SteamId?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

        var model = new AntiCheatAdminModel(access, shownList, new PagedListModel<AntiCheatAccessPlayerResult>(filtered, page, PageSize), q);

        return View(model);
    }
}
