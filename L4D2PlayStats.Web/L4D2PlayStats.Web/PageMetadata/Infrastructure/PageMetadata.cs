using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.PageMetadata.Infrastructure;

public abstract class PageMetadata
{
    private const string DefaultImageUrl = "https://l4d2playstats.blob.core.windows.net/assets/motd.jpeg";
    private const string SiteUrl = "https://l4d2.com.br";

    public static string SiteName => "L4D2 Competitive";
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string Image { get; private set; } = DefaultImageUrl;
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
        Image = ResolveImage(image);
        ImageAlt = imageAlt ?? title;
        OpenGraphType = openGraphType;
        NoIndex = noIndex;
    }

    public string GetCanonicalUrl(PathString path)
    {
        return $"{SiteUrl}{path}";
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

    private static string ResolveImage(string? image)
    {
        if (string.IsNullOrWhiteSpace(image))
            return DefaultImageUrl;

        return Uri.TryCreate(image, UriKind.Absolute, out _)
            ? image
            : $"{SiteUrl}/{image.TrimStart('/')}";
    }
}