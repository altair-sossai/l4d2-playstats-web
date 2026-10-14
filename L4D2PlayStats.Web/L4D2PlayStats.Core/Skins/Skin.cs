using L4D2PlayStats.Core.Infrastructure.Helpers;
using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.Skins;

public class Skin
{
    private const int DownloadDaysBeforeRelease = 7;

    public string Id { get; init; } = string.Empty;
    public string Weapon { get; init; } = string.Empty;
    public int Level { get; init; }
    public DateOnly ReleaseDate { get; init; }
    public DateOnly DownloadDate => ReleaseDate.AddDays(-DownloadDaysBeforeRelease);
    public DateTimeOffset ReleaseAt => BrasiliaTime.StartOfDay(ReleaseDate);
    public DateTimeOffset DownloadAt => BrasiliaTime.StartOfDay(DownloadDate);
    public bool IsDownloadable => DownloadAt <= DateTimeOffset.UtcNow;
    public bool IsReleased => ReleaseAt <= DateTimeOffset.UtcNow;
    public string WorkshopId { get; init; } = string.Empty;
    public LocalizedText Title { get; init; } = new();
    public LocalizedText Description { get; init; } = new();
    public string OriginalTitle { get; init; } = string.Empty;
    public string OriginalDescription { get; init; } = string.Empty;
    public string Image => $"/imgs/skins/{Id}.jpg";
    public string WorkshopUrl => $"https://steamcommunity.com/sharedfiles/filedetails/?id={WorkshopId}";
    public string Command => $"!skin {Id}";
}