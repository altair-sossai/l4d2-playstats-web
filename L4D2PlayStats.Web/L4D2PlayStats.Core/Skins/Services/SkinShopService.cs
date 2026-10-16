using System.Collections.Concurrent;
using Azure;
using Azure.Data.Tables;
using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Skins.Enums;
using L4D2PlayStats.Core.Skins.Models;
using L4D2PlayStats.Core.Skins.Repositories;
using L4D2PlayStats.Core.Skins.Results;
using L4D2PlayStats.Sdk.Stars;
using L4D2PlayStats.Sdk.Stars.Results;
using Microsoft.Extensions.Logging;

namespace L4D2PlayStats.Core.Skins.Services;

public class SkinShopService(
    IAppOptionsWraper config,
    ISkinService skinService,
    ISkinShopRepository repository,
    IStarsService starsService,
    ILogger<SkinShopService> logger) : ISkinShopService
{
    private const int MaxAttempts = 3;
    private const int ConflictStatus = 409;
    private const int PreconditionFailedStatus = 412;
    private const int RefundAllParallelism = 8;
    private const string SeasonRefundSource = "season";

    public async Task<WalletResult?> GetWalletAsync(string communityId, CancellationToken cancellationToken = default)
    {
        var stars = await GetStarsAsync(communityId, cancellationToken);
        if (stars == null)
            return null;

        var wallet = await repository.FindWalletAsync(communityId, cancellationToken);

        return ToWallet(stars, wallet?.Spent ?? 0);
    }

    public async Task<PlayerSkinsResult> GetPlayerSkinsAsync(string communityId, CancellationToken cancellationToken = default)
    {
        var purchases = await repository.GetPurchasesAsync(communityId, cancellationToken);
        var equipment = await repository.GetEquipmentAsync(communityId, cancellationToken);

        return ToPlayerSkins(communityId, purchases, equipment);
    }

    public async Task<List<PlayerSkinsResult>> GetAllPlayerSkinsAsync(CancellationToken cancellationToken = default)
    {
        var purchases = await repository.GetAllPurchasesAsync(cancellationToken);
        var equipment = await repository.GetAllEquipmentAsync(cancellationToken);

        var purchasesByPlayer = purchases.ToLookup(purchase => purchase.PartitionKey);
        var equipmentByPlayer = equipment.ToLookup(item => item.PartitionKey);

        return
        [
            .. purchasesByPlayer
                .Select(group => ToPlayerSkins(group.Key, group, equipmentByPlayer[group.Key]))
                .OrderBy(player => player.CommunityId)
        ];
    }

    public Task<List<SkinPurchaseEntity>> GetAllPurchasesAsync(CancellationToken cancellationToken = default)
    {
        return repository.GetAllPurchasesAsync(cancellationToken);
    }

    public async Task<SkinShopResult> BuyAsync(string communityId, string? playerName, string skinId, CancellationToken cancellationToken = default)
    {
        var skin = await skinService.FindAsync(skinId, cancellationToken);
        if (skin == null)
            return SkinShopResult.Of(SkinShopStatus.SkinNotFound);

        if (await repository.FindPurchaseAsync(communityId, skin.Id, cancellationToken) != null)
            return SkinShopResult.Of(SkinShopStatus.AlreadyOwned, skin);

        var stars = await GetStarsAsync(communityId, cancellationToken);
        if (stars == null)
            return SkinShopResult.Of(SkinShopStatus.StarsUnavailable, skin);

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            var wallet = await repository.FindWalletAsync(communityId, cancellationToken) ?? new SkinWalletEntity { PartitionKey = communityId };
            var balance = stars.Stars - wallet.Spent;

            if (balance < skin.Price)
                return SkinShopResult.Of(SkinShopStatus.InsufficientStars, skin, ToWallet(stars, wallet.Spent));

            var now = DateTimeOffset.UtcNow;
            var walletAction = wallet.ETag == default
                ? new TableTransactionAction(TableTransactionActionType.Add, wallet)
                : new TableTransactionAction(TableTransactionActionType.UpdateReplace, wallet, wallet.ETag);

            wallet.Spent += skin.Price;
            wallet.UpdatedAtUtc = now;

            var purchase = new SkinPurchaseEntity
            {
                PartitionKey = communityId,
                RowKey = SkinPurchaseEntity.RowKeyOf(skin.Id),
                SkinId = skin.Id,
                Weapon = skin.Weapon,
                Price = skin.Price,
                StarsBefore = balance,
                PlayerName = playerName,
                PurchasedAtUtc = now
            };

            var actions = new List<TableTransactionAction>
            {
                new(TableTransactionActionType.Add, purchase),
                walletAction,
                new(TableTransactionActionType.UpsertReplace, ToEquipment(communityId, skin, "shop", now))
            };

            try
            {
                await repository.SubmitAsync(actions, cancellationToken);
            }
            catch (TableTransactionFailedException exception) when (exception is { FailedTransactionActionIndex: 0, Status: ConflictStatus })
            {
                return SkinShopResult.Of(SkinShopStatus.AlreadyOwned, skin);
            }
            catch (TableTransactionFailedException exception) when (exception is { FailedTransactionActionIndex: 1, Status: ConflictStatus or PreconditionFailedStatus })
            {
                logger.LogWarning("Skin purchase conflict for {CommunityId} buying {SkinId}, attempt {Attempt}", communityId, skin.Id, attempt);
                continue;
            }

            logger.LogInformation("Skin purchased: {CommunityId} bought {SkinId} for {Price} stars (balance {Balance} -> {NewBalance})",
                communityId, skin.Id, skin.Price, balance, balance - skin.Price);

            return SkinShopResult.Of(SkinShopStatus.Success, skin, ToWallet(stars, wallet.Spent));
        }

        return SkinShopResult.Of(SkinShopStatus.Busy, skin);
    }

    public async Task<SkinShopResult> EquipAsync(string communityId, string skinId, string source, CancellationToken cancellationToken = default)
    {
        var skin = await skinService.FindAsync(skinId, cancellationToken);
        if (skin == null)
            return SkinShopResult.Of(SkinShopStatus.SkinNotFound);

        if (await repository.FindPurchaseAsync(communityId, skin.Id, cancellationToken) == null)
            return SkinShopResult.Of(SkinShopStatus.NotOwned, skin);

        var actions = new List<TableTransactionAction>
        {
            new(TableTransactionActionType.UpsertReplace, ToEquipment(communityId, skin, source, DateTimeOffset.UtcNow))
        };

        await repository.SubmitAsync(actions, cancellationToken);

        return SkinShopResult.Of(SkinShopStatus.Success, skin);
    }

    public async Task<SkinShopResult> UnequipAsync(string communityId, string weapon, string source, CancellationToken cancellationToken = default)
    {
        var skins = await skinService.GetAllSkinsAsync(cancellationToken);
        if (skins.All(skin => skin.Weapon != weapon))
            return SkinShopResult.Of(SkinShopStatus.SkinNotFound);

        var equipment = await repository.FindEquipmentAsync(communityId, weapon, cancellationToken);
        if (equipment == null)
            return SkinShopResult.Of(SkinShopStatus.Success);

        var actions = new List<TableTransactionAction>
        {
            new(TableTransactionActionType.Delete, equipment, ETag.All)
        };

        try
        {
            await repository.SubmitAsync(actions, cancellationToken);
        }
        catch (TableTransactionFailedException exception) when (exception.Status == 404)
        {
            return SkinShopResult.Of(SkinShopStatus.Success);
        }

        return SkinShopResult.Of(SkinShopStatus.Success);
    }

    public async Task<SkinShopResult> GiftAsync(string communityId, string skinId, string adminCommunityId, string? adminName, CancellationToken cancellationToken = default)
    {
        var skin = await skinService.FindAsync(skinId, cancellationToken);
        if (skin == null)
            return SkinShopResult.Of(SkinShopStatus.SkinNotFound);

        if (await repository.FindPurchaseAsync(communityId, skin.Id, cancellationToken) != null)
            return SkinShopResult.Of(SkinShopStatus.AlreadyOwned, skin);

        var now = DateTimeOffset.UtcNow;

        var purchase = new SkinPurchaseEntity
        {
            PartitionKey = communityId,
            RowKey = SkinPurchaseEntity.RowKeyOf(skin.Id),
            SkinId = skin.Id,
            Weapon = skin.Weapon,
            Price = 0,
            Gift = true,
            GiftedByCommunityId = adminCommunityId,
            GiftedByName = adminName,
            PurchasedAtUtc = now
        };

        var actions = new List<TableTransactionAction>
        {
            new(TableTransactionActionType.Add, purchase),
            new(TableTransactionActionType.UpsertReplace, ToEquipment(communityId, skin, "gift", now))
        };

        try
        {
            await repository.SubmitAsync(actions, cancellationToken);
        }
        catch (TableTransactionFailedException exception) when (exception is { FailedTransactionActionIndex: 0, Status: ConflictStatus })
        {
            return SkinShopResult.Of(SkinShopStatus.AlreadyOwned, skin);
        }

        logger.LogInformation("Skin gifted: {AdminCommunityId} gave {SkinId} to {CommunityId}", adminCommunityId, skin.Id, communityId);

        return SkinShopResult.Of(SkinShopStatus.Success, skin);
    }

    public Task<SkinShopResult> RefundAsync(string communityId, string skinId, string adminCommunityId, CancellationToken cancellationToken = default)
    {
        return RefundAsync(communityId, skinId, adminCommunityId, _ => true, cancellationToken);
    }

    private async Task<SkinShopResult> RefundAsync(string communityId, string skinId, string adminCommunityId, Func<SkinPurchaseEntity, bool> canRefund, CancellationToken cancellationToken)
    {
        var skin = await skinService.FindAsync(skinId, cancellationToken);

        for (var attempt = 1; attempt <= MaxAttempts; attempt++)
        {
            var purchase = await repository.FindPurchaseAsync(communityId, skinId, cancellationToken);
            if (purchase == null || !canRefund(purchase))
                return SkinShopResult.Of(SkinShopStatus.NotOwned, skin);

            var actions = new List<TableTransactionAction>
            {
                new(TableTransactionActionType.Delete, purchase, purchase.ETag)
            };

            if (purchase.Price > 0)
            {
                var wallet = await repository.FindWalletAsync(communityId, cancellationToken);

                if (wallet != null)
                {
                    wallet.Spent = Math.Max(0, wallet.Spent - purchase.Price);
                    wallet.UpdatedAtUtc = DateTimeOffset.UtcNow;

                    actions.Add(new TableTransactionAction(TableTransactionActionType.UpdateReplace, wallet, wallet.ETag));
                }
            }

            var equipment = await repository.FindEquipmentAsync(communityId, purchase.Weapon, cancellationToken);
            if (equipment != null && equipment.SkinId == purchase.SkinId)
                actions.Add(new TableTransactionAction(TableTransactionActionType.Delete, equipment, equipment.ETag));

            try
            {
                await repository.SubmitAsync(actions, cancellationToken);
            }
            catch (TableTransactionFailedException exception) when (exception.Status is ConflictStatus or PreconditionFailedStatus or 404)
            {
                logger.LogWarning("Skin refund conflict for {CommunityId} refunding {SkinId}, attempt {Attempt}", communityId, skinId, attempt);
                continue;
            }

            logger.LogInformation("Skin refunded: {AdminCommunityId} refunded {SkinId} ({Price} stars) to {CommunityId}",
                adminCommunityId, skinId, purchase.Price, communityId);

            return SkinShopResult.Of(SkinShopStatus.Success, skin);
        }

        return SkinShopResult.Of(SkinShopStatus.Busy, skin);
    }

    public async Task<RefundAllResult> RefundAllAsync(string adminCommunityId, CancellationToken cancellationToken = default)
    {
        var purchases = await repository.GetAllPurchasesAsync(cancellationToken);
        var summary = await RefundPurchasesAsync(purchases, adminCommunityId, _ => true, cancellationToken);

        logger.LogWarning("All skin purchases refunded by {AdminCommunityId}: {Purchases} purchases, {Gifts} gifts, {Stars} stars, {Players} players, {Failed} failed",
            adminCommunityId, summary.Purchases, summary.Gifts, summary.Stars, summary.Players, summary.Failed);

        return summary;
    }

    public async Task<RefundAllResult> RefundSeasonAsync(DateTimeOffset seasonStart, CancellationToken cancellationToken = default)
    {
        var purchases = await repository.GetAllPurchasesAsync(cancellationToken);
        var expired = purchases.Where(purchase => IsExpired(purchase, seasonStart)).ToList();

        if (expired.Count == 0)
            return new RefundAllResult();

        var summary = await RefundPurchasesAsync(expired, SeasonRefundSource, purchase => IsExpired(purchase, seasonStart), cancellationToken);

        logger.LogWarning("Skin purchases made before the season of {SeasonStart} refunded: {Purchases} purchases, {Stars} stars, {Players} players, {Failed} failed",
            seasonStart, summary.Purchases, summary.Stars, summary.Players, summary.Failed);

        return summary;
    }

    private static bool IsExpired(SkinPurchaseEntity purchase, DateTimeOffset seasonStart)
    {
        return !purchase.Gift && purchase.PurchasedAtUtc < seasonStart;
    }

    private async Task<RefundAllResult> RefundPurchasesAsync(IEnumerable<SkinPurchaseEntity> purchases, string adminCommunityId, Func<SkinPurchaseEntity, bool> canRefund, CancellationToken cancellationToken)
    {
        var refunded = new ConcurrentBag<SkinPurchaseEntity>();
        var failed = 0;

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = RefundAllParallelism,
            CancellationToken = cancellationToken
        };

        await Parallel.ForEachAsync(purchases.GroupBy(purchase => purchase.PartitionKey), options, async (player, token) =>
        {
            foreach (var purchase in player)
            {
                var result = await RefundAsync(purchase.PartitionKey, purchase.SkinId, adminCommunityId, canRefund, token);

                if (result.Succeeded)
                {
                    refunded.Add(purchase);
                    continue;
                }

                if (result.Status != SkinShopStatus.NotOwned)
                    Interlocked.Increment(ref failed);
            }
        });

        return new RefundAllResult
        {
            Purchases = refunded.Count(purchase => !purchase.Gift),
            Gifts = refunded.Count(purchase => purchase.Gift),
            Stars = refunded.Sum(purchase => purchase.Price),
            Players = refunded.Select(purchase => purchase.PartitionKey).Distinct().Count(),
            Failed = failed
        };
    }

    private async Task<PlayerStarsResult?> GetStarsAsync(string communityId, CancellationToken cancellationToken)
    {
        if (!long.TryParse(communityId, out var id))
            return null;

        try
        {
            return await starsService.GetAsync(config.ServerId, id, cancellationToken);
        }
        catch (Exception exception)
        {
            logger.LogWarning(exception, "Could not load the stars of {CommunityId}", communityId);

            return null;
        }
    }

    private static WalletResult ToWallet(PlayerStarsResult stars, int spent)
    {
        return new WalletResult
        {
            MvpSiDamage = stars.MvpSiDamage,
            MvpCommon = stars.MvpCommon,
            Stars = stars.Stars,
            Spent = spent
        };
    }

    private static SkinEquipmentEntity ToEquipment(string communityId, Skin skin, string source, DateTimeOffset now)
    {
        return new SkinEquipmentEntity
        {
            PartitionKey = communityId,
            RowKey = SkinEquipmentEntity.RowKeyOf(skin.Weapon),
            Weapon = skin.Weapon,
            SkinId = skin.Id,
            Source = source,
            UpdatedAtUtc = now
        };
    }

    private static PlayerSkinsResult ToPlayerSkins(string communityId, IEnumerable<SkinPurchaseEntity> purchases, IEnumerable<SkinEquipmentEntity> equipment)
    {
        var owned = purchases.Select(purchase => purchase.SkinId).ToHashSet();

        return new PlayerSkinsResult
        {
            CommunityId = communityId,
            Owned = owned,
            Equipped = equipment
                .Where(item => owned.Contains(item.SkinId))
                .ToDictionary(item => item.Weapon, item => item.SkinId)
        };
    }
}