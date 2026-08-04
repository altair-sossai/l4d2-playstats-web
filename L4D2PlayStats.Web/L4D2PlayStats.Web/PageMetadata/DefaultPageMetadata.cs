using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class DefaultPageMetadata : Infrastructure.PageMetadata
{
    public DefaultPageMetadata(IStringLocalizer<SharedResource> localizer)
    {
        Initialize(
            SiteName,
            localizer["DefaultMetaDescription"].Value);
    }
}