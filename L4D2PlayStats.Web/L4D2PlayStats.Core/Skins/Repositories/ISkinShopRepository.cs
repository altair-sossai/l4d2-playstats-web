using Azure.Data.Tables;
using L4D2PlayStats.Core.Skins.Models;

namespace L4D2PlayStats.Core.Skins.Repositories;

public interface ISkinShopRepository
{
    Task<SkinWalletEntity?> FindWalletAsync(string communityId, CancellationToken cancellationToken = default);
    Task<SkinPurchaseEntity?> FindPurchaseAsync(string communityId, string skinId, CancellationToken cancellationToken = default);
    Task<SkinEquipmentEntity?> FindEquipmentAsync(string communityId, string weapon, CancellationToken cancellationToken = default);
    Task<List<SkinPurchaseEntity>> GetPurchasesAsync(string communityId, CancellationToken cancellationToken = default);
    Task<List<SkinEquipmentEntity>> GetEquipmentAsync(string communityId, CancellationToken cancellationToken = default);
    Task<List<SkinPurchaseEntity>> GetAllPurchasesAsync(CancellationToken cancellationToken = default);
    Task<List<SkinEquipmentEntity>> GetAllEquipmentAsync(CancellationToken cancellationToken = default);
    Task SubmitAsync(IReadOnlyCollection<TableTransactionAction> actions, CancellationToken cancellationToken = default);
}