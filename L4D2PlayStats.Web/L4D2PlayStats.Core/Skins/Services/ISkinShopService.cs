using L4D2PlayStats.Core.Skins.Models;
using L4D2PlayStats.Core.Skins.Results;

namespace L4D2PlayStats.Core.Skins.Services;

public interface ISkinShopService
{
    Task<WalletResult?> GetWalletAsync(string communityId, CancellationToken cancellationToken = default);
    Task<PlayerSkinsResult> GetPlayerSkinsAsync(string communityId, CancellationToken cancellationToken = default);
    Task<List<PlayerSkinsResult>> GetAllPlayerSkinsAsync(CancellationToken cancellationToken = default);
    Task<List<SkinPurchaseEntity>> GetAllPurchasesAsync(CancellationToken cancellationToken = default);
    Task<SkinShopResult> BuyAsync(string communityId, string? playerName, string skinId, CancellationToken cancellationToken = default);
    Task<SkinShopResult> EquipAsync(string communityId, string skinId, string source, CancellationToken cancellationToken = default);
    Task<SkinShopResult> UnequipAsync(string communityId, string weapon, string source, CancellationToken cancellationToken = default);
    Task<SkinShopResult> GiftAsync(string communityId, string skinId, string adminCommunityId, string? adminName, CancellationToken cancellationToken = default);
    Task<SkinShopResult> RefundAsync(string communityId, string skinId, string adminCommunityId, CancellationToken cancellationToken = default);
    Task<RefundAllResult> RefundAllAsync(string adminCommunityId, CancellationToken cancellationToken = default);
}