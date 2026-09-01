using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Sdk.Ranking.Results;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class HistoryPageMetadata : Infrastructure.PageMetadata
{
    public HistoryPageMetadata(HistoryModel model, IStringLocalizer<SharedResource> localizer, IUserAvatar userAvatar)
    {
        var winner = model.Players.MinBy(player => player.Position);
        var period = PeriodLabel(model.History, localizer);

        Initialize(
            Join(localizer["History"].Value, period),
            Format(localizer, "HistoryMetaDescription", period),
            winner == null ? null : userAvatar[winner.CommunityId],
            winner?.Name);
    }

    private static string PeriodLabel(HistoryResult history, IStringLocalizer<SharedResource> localizer)
    {
        if (history.IsBimonthly)
            return Format(localizer, "PeriodRange", history.StartYear, history.StartMonth, history.EndYear, history.EndMonth);

        if (history.IsAnnual)
            return Format(localizer, "YearOf", history.StartYear);

        if (history.IsAllTime)
            return localizer["AllTime"].Value;

        return history.Id;
    }
}