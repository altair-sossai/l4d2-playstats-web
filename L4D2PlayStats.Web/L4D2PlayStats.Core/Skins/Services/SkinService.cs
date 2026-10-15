using System.Text.Json;
using System.Text.Json.Serialization;
using L4D2PlayStats.Core.Skins.Models;
using L4D2PlayStats.Core.Skins.Repositories;

namespace L4D2PlayStats.Core.Skins.Services;

public class SkinService(ISkinPriceRepository skinPriceRepository) : ISkinService
{
    public const int MinPrice = 1;
    public const int MaxPrice = 100000;

    private const string CatalogResourceName = "L4D2PlayStats.Core.Skins.Data.skins.json";

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };

    private static readonly Lazy<IReadOnlyCollection<Skin>> Catalog = new(LoadCatalog);

    public async Task<IReadOnlyCollection<Skin>> GetAllSkinsAsync(CancellationToken cancellationToken = default)
    {
        var prices = await skinPriceRepository.GetAllAsync(cancellationToken);
        var customPrices = prices.ToDictionary(price => price.RowKey, price => price.Price);

        return
        [
            .. Catalog.Value.Select(skin => customPrices.TryGetValue(skin.Id, out var price) ? skin with { CustomPrice = price } : skin)
        ];
    }

    public async Task<Skin?> FindAsync(string skinId, CancellationToken cancellationToken = default)
    {
        var skins = await GetAllSkinsAsync(cancellationToken);

        return skins.FirstOrDefault(skin => skin.Id == skinId);
    }

    public async Task SetPriceAsync(string skinId, int? price, string adminCommunityId, CancellationToken cancellationToken = default)
    {
        var skin = Catalog.Value.FirstOrDefault(skin => skin.Id == skinId) ?? throw new InvalidOperationException($"Skin '{skinId}' not found.");

        if (price == null || price == skin.DefaultPrice)
        {
            await skinPriceRepository.DeleteAsync(skin.Id, cancellationToken);
            return;
        }

        if (price is < MinPrice or > MaxPrice)
            throw new ArgumentOutOfRangeException(nameof(price));

        var entity = new SkinPriceEntity
        {
            PartitionKey = SkinPriceEntity.DefaultPartitionKey,
            RowKey = skin.Id,
            Price = price.Value,
            UpdatedByCommunityId = adminCommunityId,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        await skinPriceRepository.UpsertAsync(entity, cancellationToken);
    }

    private static IReadOnlyCollection<Skin> LoadCatalog()
    {
        using var stream = typeof(SkinService).Assembly.GetManifestResourceStream(CatalogResourceName);

        if (stream == null)
            throw new InvalidOperationException($"Embedded resource '{CatalogResourceName}' not found.");

        var catalog = JsonSerializer.Deserialize<SkinCatalog>(stream, JsonOptions);

        return catalog?.Skins ?? [];
    }
}