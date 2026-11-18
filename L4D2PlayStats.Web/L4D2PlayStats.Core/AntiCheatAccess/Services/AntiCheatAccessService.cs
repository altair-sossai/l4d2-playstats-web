using System.Globalization;
using L4D2PlayStats.Core.AntiCheatAccess.Enums;
using L4D2PlayStats.Core.AntiCheatAccess.Models;
using L4D2PlayStats.Core.AntiCheatAccess.Repositories;
using L4D2PlayStats.Core.AntiCheatAccess.Results;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.Players.Services;
using L4D2PlayStats.Core.UserAvatar;
using Serilog;

namespace L4D2PlayStats.Core.AntiCheatAccess.Services;

public class AntiCheatAccessService(
    IAntiCheatAccessSettingsRepository settingsRepository,
    IAntiCheatWhitelistRepository whitelistRepository,
    IAntiCheatRequiredRepository requiredRepository,
    IPlayerLookupService playerLookupService,
    IUserAvatar userAvatar) : IAntiCheatAccessService
{
    private const string SyncCommand = "sm_aac sync";
    private const int MaxNameLength = 64;

    public async Task<AntiCheatAccessResult> GetAsync(bool loadAvatars = true, CancellationToken cancellationToken = default)
    {
        var settings = await settingsRepository.GetAsync(cancellationToken);
        var whitelist = await PlayersAsync(AntiCheatAccessList.Whitelist, loadAvatars, cancellationToken);
        var required = await PlayersAsync(AntiCheatAccessList.Required, loadAvatars, cancellationToken);
        var saved = AntiCheatAccessNames.TryParseMode(settings?.Mode, out var mode);

        return new AntiCheatAccessResult
        {
            Mode = mode,
            ModeSaved = saved,
            UpdatedByName = settings?.UpdatedByName,
            UpdatedFrom = settings?.UpdatedFrom,
            UpdatedAtUtc = settings?.UpdatedAtUtc?.UtcDateTime,
            Whitelist = whitelist,
            Required = required
        };
    }

    public async Task<AntiCheatAccessServerResult> GetServerAsync(CancellationToken cancellationToken = default)
    {
        var access = await GetAsync(false, cancellationToken);

        return new AntiCheatAccessServerResult
        {
            Mode = access.ModeSaved ? access.Mode.ToName() : null,
            Whitelist = [.. access.Whitelist.Select(player => new AntiCheatAccessServerResult.ServerPlayer(player))],
            Required = [.. access.Required.Select(player => new AntiCheatAccessServerResult.ServerPlayer(player))]
        };
    }

    public async Task SetModeAsync(AntiCheatAccessMode mode, User actor, AntiCheatAccessSource source, CancellationToken cancellationToken = default)
    {
        var entity = new AntiCheatAccessSettingsEntity
        {
            Mode = mode.ToName(),
            UpdatedByCommunityId = actor.CommunityId,
            UpdatedByName = Truncate(actor.Name),
            UpdatedFrom = source.ToString(),
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        await settingsRepository.SaveAsync(entity, cancellationToken);

        Log.Information("Anti-cheat mode set to {Mode} by {Admin} ({AdminCommunityId}) from the {Source}", entity.Mode, actor.Name, actor.CommunityId, source);

        RequestServerSync(source);
    }

    public async Task<AntiCheatAccessPlayerResult> AddAsync(AntiCheatAccessList list, string? player, string? name, User actor, AntiCheatAccessSource source, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(player))
            throw new ArgumentException("The Steam ID must be provided.");

        if (!SteamIdentifiers.TryParse(player.Trim(), out var identifiers) || identifiers.CommunityId is not > 0)
            throw new ArgumentException("The Steam ID is invalid.");

        var repository = Repository(list);
        var rowKey = identifiers.CommunityId.Value.ToString(CultureInfo.InvariantCulture);
        var existing = (await repository.GetAllAsync(cancellationToken)).FirstOrDefault(entity => entity.RowKey == rowKey);

        if (existing != null)
            return await ResultAsync(existing, cancellationToken);

        if (string.IsNullOrWhiteSpace(name))
            name = (await playerLookupService.LookupAsync(rowKey, cancellationToken))?.Name;

        var created = new AntiCheatAccessPlayerEntity
        {
            RowKey = rowKey,
            LastName = Truncate(name) ?? string.Empty,
            SteamId = identifiers.SteamId,
            Steam3 = identifiers.Steam3,
            ProfileUrl = identifiers.ProfileUrl,
            AddedByCommunityId = actor.CommunityId,
            AddedByName = Truncate(actor.Name),
            AddedFrom = source.ToString(),
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        await repository.AddAsync(created, cancellationToken);

        Log.Information("Player {CommunityId} added to the anti-cheat {List} by {Admin} ({AdminCommunityId}) from the {Source}", rowKey, list.ToName(), actor.Name, actor.CommunityId, source);

        RequestServerSync(source);

        return await ResultAsync(created, cancellationToken);
    }

    public async Task<bool> RemoveAsync(AntiCheatAccessList list, string? player, AntiCheatAccessSource source, CancellationToken cancellationToken = default)
    {
        if (!SteamIdentifiers.TryParse(player?.Trim(), out var identifiers) || identifiers.CommunityId is not > 0)
            return false;

        var repository = Repository(list);
        var rowKey = identifiers.CommunityId.Value.ToString(CultureInfo.InvariantCulture);
        var entity = (await repository.GetAllAsync(cancellationToken)).FirstOrDefault(item => item.RowKey == rowKey);

        if (entity == null)
            return false;

        await repository.DeleteAsync(entity, cancellationToken);

        Log.Information("Player {CommunityId} removed from the anti-cheat {List} from the {Source}", rowKey, list.ToName(), source);

        RequestServerSync(source);

        return true;
    }

    private IAntiCheatAccessPlayerRepository Repository(AntiCheatAccessList list)
    {
        return list == AntiCheatAccessList.Whitelist ? whitelistRepository : requiredRepository;
    }

    private async Task<List<AntiCheatAccessPlayerResult>> PlayersAsync(AntiCheatAccessList list, bool loadAvatars, CancellationToken cancellationToken)
    {
        var entities = await Repository(list).GetAllAsync(cancellationToken);

        var players = entities
            .OrderByDescending(entity => entity.CreatedAtUtc)
            .Select(entity => new AntiCheatAccessPlayerResult(entity))
            .ToList();

        if (!loadAvatars)
            return players;

        await userAvatar.LoadAsync(players.Select(player => player.CommunityId), cancellationToken: cancellationToken);

        foreach (var player in players)
            player.UpdateAvatarUrl(userAvatar);

        return players;
    }

    private async Task<AntiCheatAccessPlayerResult> ResultAsync(AntiCheatAccessPlayerEntity entity, CancellationToken cancellationToken)
    {
        var result = new AntiCheatAccessPlayerResult(entity);

        await userAvatar.LoadAsync(result.CommunityId, cancellationToken: cancellationToken);

        result.UpdateAvatarUrl(userAvatar);

        return result;
    }

    private void RequestServerSync(AntiCheatAccessSource source)
    {
        if (source != AntiCheatAccessSource.Site || !ServerCommand.TryParse(SyncCommand, out var command) || command == null)
            return;

        var commands = GameInfo.GameInfo.GetOrInitializeInstance(userAvatar).ServerCommands;

        lock (commands)
        {
            if (commands.All(queued => queued.FullCommand != command.FullCommand))
                commands.Enqueue(command);
        }
    }

    private static string? Truncate(string? value)
    {
        var trimmed = value?.Trim();

        return string.IsNullOrEmpty(trimmed) || trimmed.Length <= MaxNameLength ? trimmed : trimmed[..MaxNameLength];
    }
}
