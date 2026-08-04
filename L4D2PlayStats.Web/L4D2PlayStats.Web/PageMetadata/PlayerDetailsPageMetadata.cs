using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class PlayerDetailsPageMetadata : Infrastructure.PageMetadata
{
    public PlayerDetailsPageMetadata(PlayerDetailsModel model, IStringLocalizer<SharedResource> localizer, IUserAvatar userAvatar)
    {
        var firstPlayer = model.FirstPlayerRanking.Player;
        var secondPlayer = model.SecondPlayerRanking?.Player;
        var firstPlayerName = firstPlayer.Name ?? firstPlayer.CommunityId.ToString();
        var secondPlayerName = secondPlayer?.Name ?? secondPlayer?.CommunityId.ToString();

        var title = secondPlayer == null
            ? Join(firstPlayerName, localizer["PlayerDetails"].Value)
            : $"{firstPlayerName} vs {secondPlayerName}";

        var description = secondPlayer == null
            ? Format(localizer, "PlayerMetaDescription", firstPlayerName, firstPlayer.Position, firstPlayer.Games, firstPlayer.Wins)
            : Format(localizer, "PlayerComparisonMetaDescription", firstPlayerName, secondPlayerName);

        Initialize(
            title,
            description,
            userAvatar[firstPlayer.CommunityId],
            firstPlayerName,
            "profile");
    }
}