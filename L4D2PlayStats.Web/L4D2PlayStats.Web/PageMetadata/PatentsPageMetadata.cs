using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class PatentsPageMetadata : Infrastructure.PageMetadata
{
    public PatentsPageMetadata(PatentModel model, IStringLocalizer<SharedResource> localizer)
    {
        var highestPatent = model.Patents.MaxBy(patent => patent.Level);

        Initialize(
            localizer["Patents"].Value,
            localizer["PatentsMetaDescription"].Value,
            highestPatent?.Image,
            highestPatent?.FullName);
    }
}