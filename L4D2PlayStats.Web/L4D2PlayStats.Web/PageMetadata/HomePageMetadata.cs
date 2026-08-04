using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class HomePageMetadata : Infrastructure.PageMetadata
{
    public HomePageMetadata(HomeModel model, IStringLocalizer<SharedResource> localizer, IUserAvatar userAvatar)
    {
        var topPlayer = model.Ranking.MinBy(ranking => ranking.Player.Position)?.Player;

        Initialize(
            localizer["Ranking"].Value,
            localizer["RankingMetaDescription"].Value,
            topPlayer == null ? null : userAvatar[topPlayer.CommunityId],
            topPlayer?.Name);
    }
}