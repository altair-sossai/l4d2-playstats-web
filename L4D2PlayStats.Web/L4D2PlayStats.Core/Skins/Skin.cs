using System.Text.Json.Serialization;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Skins.Enums;

namespace L4D2PlayStats.Core.Skins;

public record Skin
{
    public string Id { get; init; } = string.Empty;
    public string Weapon { get; init; } = string.Empty;
    public SkinRarity Rarity { get; init; }

    [JsonPropertyName("price")]
    public int DefaultPrice { get; init; }

    [JsonIgnore]
    public int? CustomPrice { get; init; }

    [JsonIgnore]
    public int Price => CustomPrice ?? DefaultPrice;

    public string WorkshopId { get; init; } = string.Empty;
    public SkinAuthor Author { get; init; } = new();
    public LocalizedText Title { get; init; } = new();
    public LocalizedText Description { get; init; } = new();
    public string OriginalTitle { get; init; } = string.Empty;
    public string OriginalDescription { get; init; } = string.Empty;
    public int Shots { get; init; }

    [JsonIgnore]
    public bool HasShots => Shots > 0;

    public string Image => $"/imgs/skins/{Id}.jpg";
    public string WorkshopUrl => $"https://steamcommunity.com/sharedfiles/filedetails/?id={WorkshopId}";
    public string Command => $"!skin {Id}";
}