using L4D2PlayStats.Core.Campaign.Contracts;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class MatchStatisticsPageMetadata : Infrastructure.PageMetadata
{
    public MatchStatisticsPageMetadata(
        StatisticsDetailsModel model,
        IStringLocalizer<SharedResource> localizer,
        ICampaignThumb campaignThumb)
    {
        var match = model.Match;
        var campaign = match.Campaign ?? localizer["Campaign"].Value;
        var map = model.Statistic?.Statistic?.GameRound?.MapName ?? localizer["Map"].Value;

        Initialize(
            Join(map, campaign),
            Format(localizer, "MapStatisticsMetaDescription", map, campaign, match.ScoreA, match.ScoreB),
            campaignThumb[match.Campaign],
            campaign);
    }
}