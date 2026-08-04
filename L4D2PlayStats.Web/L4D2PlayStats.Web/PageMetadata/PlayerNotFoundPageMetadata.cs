using L4D2PlayStats.Core.UserAvatar;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class PlayerNotFoundPageMetadata : Infrastructure.PageMetadata
{
    public PlayerNotFoundPageMetadata(PlayerNotFoundModel model, IStringLocalizer<SharedResource> localizer, IUserAvatar userAvatar)
    {
        var playerName = model.Name ?? model.CommunityId.ToString();

        Initialize(
            Join(playerName, localizer["PlayerDetails"].Value),
            Format(localizer, "PlayerNotFoundMetaDescription", playerName),
            userAvatar[model.CommunityId],
            playerName,
            "profile",
            true);
    }
}