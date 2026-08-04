using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class PlayersPageMetadata : Infrastructure.PageMetadata
{
    public PlayersPageMetadata(PlayersModel model, IStringLocalizer<SharedResource> localizer, IUserAvatar userAvatar)
    {
        var topPlayer = model.Players.FirstOrDefault();

        Initialize(
            localizer["Players"].Value,
            Format(localizer, "PlayersMetaDescription", model.Players.Count),
            topPlayer == null ? null : userAvatar[topPlayer.CommunityId],
            topPlayer?.Name);
    }
}