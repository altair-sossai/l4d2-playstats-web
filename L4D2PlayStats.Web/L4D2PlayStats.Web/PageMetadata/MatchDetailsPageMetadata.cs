using L4D2PlayStats.Core.Campaign.Contracts;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class MatchDetailsPageMetadata : Infrastructure.PageMetadata
{
    public MatchDetailsPageMetadata(
        MatchDetailsModel model,
        IStringLocalizer<SharedResource> localizer,
        ICampaignThumb campaignThumb)
    {
        var match = model.Match;
        var campaign = match.Campaign ?? localizer["Campaign"].Value;

        Initialize(
            Join(localizer["MatchDetails"].Value, campaign),
            Format(localizer, "MatchMetaDescription", campaign, match.MatchStart.ToString("d"), match.ScoreA, match.ScoreB),
            campaignThumb[match.Campaign],
            campaign);
    }
}