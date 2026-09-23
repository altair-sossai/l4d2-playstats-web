using L4D2PlayStats.Core;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata.Infrastructure;

public abstract class PageMetadata
{
    public static string SiteName => "L4D2 Competitive";
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string ImageUrl { get; private set; } = AppConsts.DefaultImageUrl;
    public string ImageAlt { get; private set; } = string.Empty;
    public string OpenGraphType { get; private set; } = "website";
    public bool NoIndex { get; private set; }
    public string Robots => NoIndex ? "noindex, follow" : "index, follow";

    protected void Initialize(
        string title,
        string description,
        string? image = null,
        string? imageAlt = null,
        string openGraphType = "website",
        bool noIndex = false)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(description);
        ArgumentException.ThrowIfNullOrWhiteSpace(openGraphType);

        Title = title == SiteName ? SiteName : $"{title} | {SiteName}";
        Description = description;
        ImageUrl = ResolveImageUrl(image);
        ImageAlt = imageAlt ?? title;
        OpenGraphType = openGraphType;
        NoIndex = noIndex;
    }

    public string GetCanonicalUrl(PathString path)
    {
        return $"{AppConsts.SiteUrl}{path}";
    }

    protected static string Format(
        IStringLocalizer<SharedResource> localizer,
        string resourceName,
        params object?[] arguments)
    {
        return string.Format(localizer[resourceName].Value, arguments);
    }

    protected static string Join(params string?[] parts)
    {
        return string.Join(" - ", parts.Where(part => !string.IsNullOrWhiteSpace(part)));
    }

    private static string ResolveImageUrl(string? image)
    {
        if (string.IsNullOrWhiteSpace(image))
            return AppConsts.DefaultImageUrl;

        return Uri.TryCreate(image, UriKind.Absolute, out _)
            ? image
            : $"{AppConsts.SiteUrl}/{image.TrimStart('/')}";
    }
}