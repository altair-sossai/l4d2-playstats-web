using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata;

public sealed class HistoryNotFoundPageMetadata : Infrastructure.PageMetadata
{
    public HistoryNotFoundPageMetadata(IStringLocalizer<SharedResource> localizer)
    {
        Initialize(
            localizer["HistoryNotFoundTitle"].Value,
            localizer["DefaultMetaDescription"].Value,
            noIndex: true);
    }
}