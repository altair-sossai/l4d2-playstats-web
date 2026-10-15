using L4D2PlayStats.Core.Skins.Models;

namespace L4D2PlayStats.Core.Skins.Repositories;

public interface ISkinPriceRepository
{
    Task<List<SkinPriceEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task UpsertAsync(SkinPriceEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(string skinId, CancellationToken cancellationToken = default);
}