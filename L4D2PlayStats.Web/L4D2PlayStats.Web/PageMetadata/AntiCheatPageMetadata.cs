using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class AntiCheatPageMetadata : Infrastructure.PageMetadata
{
    public AntiCheatPageMetadata(IStringLocalizer<SharedResource> localizer)
    {
        Initialize(
            localizer["AntiCheat"].Value,
            localizer["AntiCheatMetaDescription"].Value);
    }
}