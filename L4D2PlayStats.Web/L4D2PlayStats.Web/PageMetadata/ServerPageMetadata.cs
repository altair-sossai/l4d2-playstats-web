using L4D2PlayStats.Core.Campaign.Contracts;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class ServerPageMetadata : Infrastructure.PageMetadata
{
    public ServerPageMetadata(
        ServerInfoModel model,
        IStringLocalizer<SharedResource> localizer,
        ICampaignName campaignName,
        ICampaignThumb campaignThumb)
    {
        var map = model.ServerInfo?.Map;
        var campaign = campaignName[map];
        var mapDescription = campaign ?? map ?? localizer["Map"].Value;

        Initialize(
            localizer["Server"].Value,
            Format(localizer, "ServerMetaDescription", mapDescription, model.ServerInfo?.Players ?? 0),
            campaignThumb[campaign],
            mapDescription);
    }
}