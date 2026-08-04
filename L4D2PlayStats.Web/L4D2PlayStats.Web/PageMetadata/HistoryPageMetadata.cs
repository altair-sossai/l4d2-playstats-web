using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class HistoryPageMetadata : Infrastructure.PageMetadata
{
    public HistoryPageMetadata(HistoryModel model, IStringLocalizer<SharedResource> localizer, IUserAvatar userAvatar)
    {
        var winner = model.Players.MinBy(player => player.Position);

        Initialize(
            Join(localizer["History"].Value, model.History.Id),
            Format(localizer, "HistoryMetaDescription", model.History.Id),
            winner == null ? null : userAvatar[winner.CommunityId],
            winner?.Name);
    }
}