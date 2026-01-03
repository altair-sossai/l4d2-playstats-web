using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class HistoryController(IRankingServiceCached rankingService, IUserAvatar userAvatar) : Controller
{
    [Route("history/{historyId?}")]
    public async Task<IActionResult> Index(string? historyId = null)
    {
        ViewBag.History = "active";

        var allHistory = await rankingService.AllHistoryAsync();

        allHistory = allHistory
            .Where(h => h.IsAnnual || h.StartYear == DateTime.Now.Year)
            .ToList();

        if (allHistory.Count == 0)
            return View("NoHistory");

        var history = allHistory.LastOrDefault();

        if (!string.IsNullOrEmpty(historyId))
            history = allHistory.FirstOrDefault(h => h.Id.Equals(historyId, StringComparison.CurrentCultureIgnoreCase));

        if (history == null)
            return View("NoHistory");

        var players = await rankingService.HistoryAsync(history.Id);
        var communityIds = players.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        var model = new HistoryModel(history, players, allHistory);

        return View(model);
    }
}