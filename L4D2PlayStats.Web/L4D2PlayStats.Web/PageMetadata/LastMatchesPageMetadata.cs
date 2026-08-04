using L4D2PlayStats.Core.Campaign.Contracts;
using L4D2PlayStats.Sdk.Matches.Results;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class LastMatchesPageMetadata : Infrastructure.PageMetadata
{
    public LastMatchesPageMetadata(
        IEnumerable<MatchResult> matches,
        IStringLocalizer<SharedResource> localizer,
        ICampaignThumb campaignThumb)
    {
        var latestMatch = matches.FirstOrDefault();

        Initialize(
            localizer["LastMatches"].Value,
            localizer["LastMatchesMetaDescription"].Value,
            latestMatch == null ? null : campaignThumb[latestMatch.Campaign],
            latestMatch?.Campaign);
    }
}