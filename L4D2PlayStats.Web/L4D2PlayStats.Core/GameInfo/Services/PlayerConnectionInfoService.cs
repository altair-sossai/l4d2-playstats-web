using System.Globalization;
using System.Net;
using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.GameInfo.Repositories;
using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.GameInfo.Services;

public class PlayerConnectionInfoService(IPlayerConnectionInfoRepository playerConnectionInfoRepository) : IPlayerConnectionInfoService
{
    private const int CleanupBatchSize = 100;

    public async Task<PlayerConnectionInfoDetailsResult> PlayerConnectionInfoDetailsAsync(long communityId, CancellationToken cancellationToken = default)
    {
        if (communityId <= 0)
            throw new ArgumentException("Community ID must be provided.", nameof(communityId));

        var rowKey = communityId.ToString(CultureInfo.InvariantCulture);
        var playerConnections = await playerConnectionInfoRepository.GetAllByCommunityIdAsync(communityId, cancellationToken);

        if (playerConnections.Count == 0)
        {
            return new PlayerConnectionInfoDetailsResult
            {
                Player = null,
                RelatedPlayers = []
            };
        }

        var ipAddresses = playerConnections.Select(connection => connection.PartitionKey).Distinct(StringComparer.Ordinal);
        var relatedPlayers = await playerConnectionInfoRepository.GetAllByIpAddressesAsync(ipAddresses, cancellationToken);
        var currentPlayer = playerConnections.OrderByDescending(connection => connection.LastConnectedAtUtc).First();

        return new PlayerConnectionInfoDetailsResult
        {
            Player = new PlayerConnectionInfoResult
            {
                CommunityId = rowKey,
                Name = currentPlayer.LastName,
                SteamId = currentPlayer.SteamId,
                Steam3 = currentPlayer.Steam3,
                ProfileUrl = currentPlayer.ProfileUrl,
                FirstConnectedAtUtc = playerConnections.Min(connection => connection.FirstConnectedAtUtc).DateTime.ToLocalTime(),
                LastConnectedAtUtc = currentPlayer.LastConnectedAtUtc.DateTime.ToLocalTime(),
                ConnectionCount = playerConnections.Sum(connection => connection.ConnectionCount)
            },
            RelatedPlayers =
            [
                .. relatedPlayers
                    .Where(player => !string.Equals(player.RowKey, rowKey, StringComparison.Ordinal))
                    .GroupBy(player => player.RowKey, StringComparer.Ordinal)
                    .Select(group =>
                    {
                        var relatedPlayer = group.OrderByDescending(player => player.LastConnectedAtUtc).First();

                        return new PlayerConnectionInfoResult
                        {
                            CommunityId = relatedPlayer.RowKey,
                            Name = relatedPlayer.LastName,
                            SteamId = relatedPlayer.SteamId,
                            Steam3 = relatedPlayer.Steam3,
                            ProfileUrl = relatedPlayer.ProfileUrl,
                            FirstConnectedAtUtc = group.Min(connection => connection.FirstConnectedAtUtc).DateTime.ToLocalTime(),
                            LastConnectedAtUtc = relatedPlayer.LastConnectedAtUtc.DateTime.ToLocalTime(),
                            ConnectionCount = group.Sum(connection => connection.ConnectionCount)
                        };
                    })
                    .OrderByDescending(player => player.LastConnectedAtUtc)
            ]
        };
    }

    public async Task<List<PlayerConnectionInfoResult>> RelatedPlayerConnectionInfoAsync(long communityId, CancellationToken cancellationToken = default)
    {
        if (communityId <= 0)
            throw new ArgumentException("Community ID must be provided.", nameof(communityId));

        var rowKey = communityId.ToString(CultureInfo.InvariantCulture);
        var playerConnections = await playerConnectionInfoRepository.GetAllByCommunityIdAsync(communityId, cancellationToken);
        var ipAddresses = playerConnections.Select(connection => connection.PartitionKey).Distinct(StringComparer.Ordinal);
        var relatedPlayers = await playerConnectionInfoRepository.GetAllByIpAddressesAsync(ipAddresses, cancellationToken);

        return
        [
            .. relatedPlayers
                .Where(player => !string.Equals(player.RowKey, rowKey, StringComparison.Ordinal))
                .GroupBy(player => player.RowKey, StringComparer.Ordinal)
                .Select(group =>
                {
                    var player = group.OrderByDescending(player => player.LastConnectedAtUtc).First();

                    return new PlayerConnectionInfoResult
                    {
                        CommunityId = player.RowKey,
                        Name = player.LastName,
                        SteamId = player.SteamId,
                        Steam3 = player.Steam3,
                        ProfileUrl = player.ProfileUrl,
                        FirstConnectedAtUtc = group.Min(connection => connection.FirstConnectedAtUtc).DateTime.ToLocalTime(),
                        LastConnectedAtUtc = player.LastConnectedAtUtc.DateTime.ToLocalTime(),
                        ConnectionCount = group.Sum(connection => connection.ConnectionCount)
                    };
                })
                .OrderByDescending(player => player.LastConnectedAtUtc)
        ];
    }

    public async Task<List<PlayerConnectionInfoResult>> AddOrUpdateAsync(PlayerConnectionInfoCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.CommunityId))
            throw new ArgumentException("Community ID must be provided.", nameof(command.CommunityId));

        if (!long.TryParse(command.CommunityId, NumberStyles.Integer, CultureInfo.InvariantCulture, out var communityId) || communityId <= 0)
            throw new ArgumentException("Community ID is invalid.", nameof(command.CommunityId));

        var rowKey = communityId.ToString(CultureInfo.InvariantCulture);

        if (string.IsNullOrWhiteSpace(command.IpAddress))
            throw new ArgumentException("IP address must be provided.", nameof(command.IpAddress));

        if (!IPAddress.TryParse(command.IpAddress, out var ipAddress))
            throw new ArgumentException("IP address is invalid.", nameof(command.IpAddress));

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name must be provided.", nameof(command.Name));

        SteamIdentifiers.TryParse(rowKey, out var steamIdentifiers);

        var connectedAtUtc = DateTimeOffset.UtcNow;
        var entity = await playerConnectionInfoRepository.FindAsync(ipAddress.ToString(), communityId, cancellationToken);

        if (entity is null)
        {
            entity = new PlayerConnectionInfoEntity
            {
                PartitionKey = ipAddress.ToString(),
                RowKey = rowKey,
                LastName = command.Name,
                SteamId = steamIdentifiers.SteamId,
                Steam3 = steamIdentifiers.Steam3,
                ProfileUrl = steamIdentifiers.ProfileUrl,
                FirstConnectedAtUtc = connectedAtUtc,
                LastConnectedAtUtc = connectedAtUtc,
                ConnectionCount = 1
            };

            await playerConnectionInfoRepository.AddAsync(entity, cancellationToken);
        }
        else
        {
            entity.LastName = command.Name;
            entity.SteamId = steamIdentifiers.SteamId;
            entity.Steam3 = steamIdentifiers.Steam3;
            entity.ProfileUrl = steamIdentifiers.ProfileUrl;
            entity.LastConnectedAtUtc = connectedAtUtc;
            entity.ConnectionCount++;

            await playerConnectionInfoRepository.UpdateAsync(entity, entity.ETag, cancellationToken);
        }

        return await RelatedPlayerConnectionInfoAsync(communityId, cancellationToken);
    }

    public async Task DeleteExpiredAsync(DateTimeOffset lastConnectedAtUtc, CancellationToken cancellationToken = default)
    {
        while (true)
        {
            var expiredConnections = await playerConnectionInfoRepository.GetAllBeforeAsync(lastConnectedAtUtc, CleanupBatchSize, cancellationToken);

            if (expiredConnections.Count == 0)
                return;

            foreach (var expiredConnection in expiredConnections)
                await playerConnectionInfoRepository.DeleteAsync(expiredConnection, cancellationToken);
        }
    }
}