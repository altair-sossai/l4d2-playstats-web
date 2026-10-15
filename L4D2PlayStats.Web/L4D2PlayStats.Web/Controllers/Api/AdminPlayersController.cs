using System.Globalization;
using L4D2PlayStats.Core.Ranking.Services;
using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Sdk.Ranking.Results;
using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/admin/players")]
[Authorize]
[AdminAuthorize]
[ApiController]
public class AdminPlayersController(IRankingServiceCached rankingService, IUserAvatar userAvatar) : ControllerBase
{
    private const string AllTimeHistoryId = "all_time";

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
        var current = await rankingService.GetAsync(cancellationToken);
        var allTime = await rankingService.HistoryAsync(AllTimeHistoryId, cancellationToken);
        var currentIds = current.Select(player => player.CommunityId).ToHashSet();
        var allTimeById = allTime.GroupBy(player => player.CommunityId).ToDictionary(group => group.Key, group => group.First());

        var players = current
            .Concat(allTime.Where(player => !currentIds.Contains(player.CommunityId)))
            .Where(player => player.CommunityId > 0)
            .ToList();

        await userAvatar.LoadAsync(players.Select(player => player.CommunityId), false, cancellationToken);

        var result = players.Select(player => new
        {
            communityId = player.CommunityId.ToString(CultureInfo.InvariantCulture),
            name = string.IsNullOrWhiteSpace(player.Name) ? player.CommunityId.ToString(CultureInfo.InvariantCulture) : player.Name,
            player.SteamId,
            position = currentIds.Contains(player.CommunityId) ? player.Position : (int?)null,
            previousNames = PreviousNames(player, allTimeById.GetValueOrDefault(player.CommunityId)),
            avatarUrl = userAvatar[player.CommunityId]
        });

        return Ok(result);
    }

    private static List<string> PreviousNames(PlayerResult player, PlayerResult? allTimePlayer)
    {
        return
        [
            .. new[] { allTimePlayer?.Name }
                .Concat((player.PreviousNames ?? []).Select(previous => previous.Name))
                .Concat((allTimePlayer?.PreviousNames ?? []).Select(previous => previous.Name))
                .OfType<string>()
                .Where(name => !string.IsNullOrWhiteSpace(name) && name != player.Name)
                .Distinct()
                .Take(5)
        ];
    }
}
