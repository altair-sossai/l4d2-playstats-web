using L4D2PlayStats.Core.Cache.Services;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Sdk.Statistics;
using L4D2PlayStats.Sdk.Statistics.Commands;
using L4D2PlayStats.Web.Attributes;
using L4D2PlayStats.Web.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/statistics")]
[Authorize]
[ApiController]
public class StatisticsController(IAppOptionsWraper config, IStatisticsService statisticsService, ICacheService cacheService) : ControllerBase
{
    [HttpPut("{statisticId}/update-score")]
    [AdminAuthorize]
    public async Task<IActionResult> UpdateScoreAsync(string statisticId, [FromBody] UpdateTeamScoreCommand command, CancellationToken cancellationToken)
    {
        var statistics = await statisticsService.GetStatisticAsync(config.ServerId, statisticId, cancellationToken);

        if (statistics == null)
            return NotFound($"Statistic with ID {statisticId} not found.");

        if (statistics.Statistic?.Scoring?.TeamA?.Score == null)
            return BadRequest("Statistic does not have scoring information for Team A.");

        if (statistics.Statistic?.Scoring?.TeamB?.Score == null)
            return BadRequest("Statistic does not have scoring information for Team B.");

        var teamAScore = command.Team == 'A' ? command.Score : statistics.Statistic.Scoring.TeamA.Score;
        var teamBScore = command.Team == 'B' ? command.Score : statistics.Statistic.Scoring.TeamB.Score;

        var updateScoreCommand = new UpdateScoreCommand
        {
            TeamAScore = teamAScore,
            TeamBScore = teamBScore
        };

        var result = await statisticsService.UpdateScoreAsync(statisticId, updateScoreCommand, cancellationToken);

        cacheService.ClearCache();

        return Ok(result);
    }
}