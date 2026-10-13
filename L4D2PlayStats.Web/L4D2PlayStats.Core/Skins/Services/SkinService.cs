using System.Text.Json;

namespace L4D2PlayStats.Core.Skins.Services;

public class SkinService : ISkinService
{
    private const string CatalogResourceName = "L4D2PlayStats.Core.Skins.Data.skins.json";

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private static readonly Lazy<IReadOnlyCollection<Skin>> Skins = new(LoadSkins);

    public IReadOnlyCollection<Skin> GetAllSkins()
    {
        return Skins.Value;
    }

    public IReadOnlyCollection<Skin> GetSkinsByLevel(int level)
    {
        return [.. Skins.Value.Where(skin => skin.Level == level)];
    }

    private static IReadOnlyCollection<Skin> LoadSkins()
    {
        using var stream = typeof(SkinService).Assembly.GetManifestResourceStream(CatalogResourceName);

        if (stream == null)
            throw new InvalidOperationException($"Embedded resource '{CatalogResourceName}' not found.");

        var catalog = JsonSerializer.Deserialize<SkinCatalog>(stream, JsonOptions);

        return catalog?.Skins ?? [];
    }
}