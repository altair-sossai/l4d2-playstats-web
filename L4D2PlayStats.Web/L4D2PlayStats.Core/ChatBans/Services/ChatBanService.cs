using System.Globalization;
using L4D2PlayStats.Core.ChatBans.Commands;
using L4D2PlayStats.Core.ChatBans.Models;
using L4D2PlayStats.Core.ChatBans.Repositories;
using L4D2PlayStats.Core.ChatBans.Results;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.UserAvatar;

namespace L4D2PlayStats.Core.ChatBans.Services;

public class ChatBanService(IChatBanRepository chatBanRepository, IUserAvatar userAvatar) : IChatBanService
{
    private const int MaxRecentSenders = 30;

    public async Task<ChatBanResult?> FindAsync(string? communityId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(communityId))
            return null;

        var entity = await chatBanRepository.FindAsync(communityId, cancellationToken);

        if (entity == null)
            return null;

        var result = new ChatBanResult(entity);

        await userAvatar.LoadAsync(result.CommunityId, cancellationToken: cancellationToken);

        result.UpdateAvatarUrl(userAvatar);

        return result;
    }

    public async Task<List<ChatBanResult>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await chatBanRepository.GetAllAsync(cancellationToken);

        var items = entities
            .OrderByDescending(entity => entity.CreatedAtUtc)
            .Select(entity => new ChatBanResult(entity))
            .ToList();

        await userAvatar.LoadAsync(items.Select(item => item.CommunityId), cancellationToken: cancellationToken);

        foreach (var item in items)
            item.UpdateAvatarUrl(userAvatar);

        return items;
    }

    public async Task<List<RecentChatSenderResult>> GetRecentSendersAsync(CancellationToken cancellationToken = default)
    {
        var gameInfo = GameInfo.GameInfo.GetOrInitializeInstance(userAvatar);

        var bannedCommunityIds = (await chatBanRepository.GetAllAsync(cancellationToken))
            .Select(entity => entity.RowKey)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var recentSenders = gameInfo.ExternalMessages
            .Where(message => !string.IsNullOrEmpty(message.CommunityId))
            .GroupBy(message => message.CommunityId, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.OrderByDescending(message => message.When).First())
            .OrderByDescending(message => message.When)
            .Take(MaxRecentSenders)
            .Select(message => new RecentChatSenderResult(message, bannedCommunityIds.Contains(message.CommunityId!)))
            .ToList();

        await userAvatar.LoadAsync(recentSenders.Select(sender => sender.CommunityId), cancellationToken: cancellationToken);

        foreach (var sender in recentSenders)
            sender.UpdateAvatarUrl(userAvatar);

        return recentSenders;
    }

    public async Task<ChatBanResult> AddOrUpdateAsync(ChatBanCommand command, User admin, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.CommunityId))
            throw new ArgumentException("Community ID must be provided.", nameof(command.CommunityId));

        if (!SteamIdentifiers.TryParse(command.CommunityId, out var steamIdentifiers) || steamIdentifiers.CommunityId is not > 0)
            throw new ArgumentException("Community ID is invalid.", nameof(command.CommunityId));

        var rowKey = steamIdentifiers.CommunityId.Value.ToString(CultureInfo.InvariantCulture);
        var entity = await chatBanRepository.FindAsync(rowKey, cancellationToken);
        var now = DateTimeOffset.UtcNow;

        if (entity == null)
        {
            entity = new ChatBanEntity
            {
                PartitionKey = ChatBanEntity.DefaultPartitionKey,
                RowKey = rowKey,
                LastName = command.Name ?? string.Empty,
                SteamId = steamIdentifiers.SteamId,
                Steam3 = steamIdentifiers.Steam3,
                ProfileUrl = steamIdentifiers.ProfileUrl,
                Reason = command.Reason,
                CreatedByCommunityId = admin.CommunityId,
                CreatedByName = admin.Name,
                CreatedAtUtc = now
            };

            await chatBanRepository.AddAsync(entity, cancellationToken);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(command.Name))
                entity.LastName = command.Name;

            entity.Reason = command.Reason;
            entity.UpdatedAtUtc = now;

            await chatBanRepository.UpdateAsync(entity, entity.ETag, cancellationToken);
        }

        var result = new ChatBanResult(entity);

        await userAvatar.LoadAsync(result.CommunityId, cancellationToken: cancellationToken);

        result.UpdateAvatarUrl(userAvatar);

        return result;
    }

    public async Task DeleteAsync(string communityId, CancellationToken cancellationToken = default)
    {
        var entity = await chatBanRepository.FindAsync(communityId, cancellationToken);

        if (entity == null)
            return;

        await chatBanRepository.DeleteAsync(entity, cancellationToken);
    }
}