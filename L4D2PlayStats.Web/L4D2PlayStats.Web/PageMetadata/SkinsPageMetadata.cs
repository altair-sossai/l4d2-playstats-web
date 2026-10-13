using System.Globalization;
using L4D2PlayStats.Web.Models;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class SkinsPageMetadata : Infrastructure.PageMetadata
{
    public SkinsPageMetadata(SkinsModel model, IStringLocalizer<SharedResource> localizer)
    {
        var highlight = model.Skins.MaxBy(skin => skin.Level);

        Initialize(
            localizer["Skins"].Value,
            localizer["SkinsMetaDescription"].Value,
            highlight?.Image,
            highlight?.Title.Get(CultureInfo.CurrentUICulture));
    }
}