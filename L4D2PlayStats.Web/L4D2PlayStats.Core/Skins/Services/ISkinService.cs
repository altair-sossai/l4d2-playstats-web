namespace L4D2PlayStats.Core.Skins.Services;

public interface ISkinService
{
    Task<IReadOnlyCollection<Skin>> GetAllSkinsAsync(CancellationToken cancellationToken = default);
    Task<Skin?> FindAsync(string skinId, CancellationToken cancellationToken = default);
    Task SetPriceAsync(string skinId, int? price, string adminCommunityId, CancellationToken cancellationToken = default);
}