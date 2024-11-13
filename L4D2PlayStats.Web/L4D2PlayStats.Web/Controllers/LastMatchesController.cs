using L4D2PlayStats.Matches;
using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Matches.Results;
using L4D2PlayStats.Sdk.Statistics;
using L4D2PlayStats.Sdk.Statistics.Results;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Mvc;
using static L4D2PlayStats.Sdk.Statistics.Results.StatisticsResult;

namespace L4D2PlayStats.Web.Controllers;

[Route("last-matches")]
public class LastMatchesController(
    IConfiguration configuration,
    IMatchesService matchesService,
    IStatisticsService statisticsService,
    IMatchesServiceCached matchesServiceCached,
    IUserAvatar userAvatar) : Controller
{
    private string ServerId => configuration.GetValue<string>("ServerId")!;

    public async Task<IActionResult> Index()
    {
        ViewBag.LastMatches = "active";

        var matches = await matchesServiceCached.GetAsync();

        await LoadAvatarsAsync(matches);

        return View(matches);
    }

    [Route("details/{start}/{end}")]
    public async Task<IActionResult> Details(string start, string end)
    {
        var matches = await matchesService.BetweenAsync(ServerId, start, end);
        var match = matches.First();
        var statistics = await statisticsService.BetweenAsync(ServerId, start, end);

        await LoadAvatarsAsync(matches);

        var model = new MatchDetailsModel(match, statistics);

        return View(model);
    }

    [Route("details/{start}/{end}/statistics/{statisticId}")]
    public async Task<IActionResult> Statistics(string start, string end, string statisticId)
    {
        var matches = await matchesService.BetweenAsync(ServerId, start, end);
        var match = matches.First();
        var statistics = await statisticsService.StatisticsAsync(ServerId, statisticId);

        await LoadAvatarsAsync(matches);
        await LoadAvatarsAsync(statistics);

        var model = new StatisticsDetailsModel(match, statistics);

        return View(model);
    }

    private async Task LoadAvatarsAsync(List<MatchResult> matches)
    {
        var communityIds = matches
            .SelectMany(m => m.Teams ?? [])
            .SelectMany(t => t.Players ?? [])
            .Select(p => p.CommunityId);

        await userAvatar.LoadAsync(communityIds);
    }

    private async Task LoadAvatarsAsync(StatisticsResult statistics)
    {
        var teams = new List<List<PlayerNameResult>?>
        {
            statistics.Statistic?.TeamA,
            statistics.Statistic?.TeamB
        };

        var communityIds = teams.SelectMany(team => team?.Select(pn => pn.CommunityId) ?? []);

        await userAvatar.LoadAsync(communityIds);
    }
}