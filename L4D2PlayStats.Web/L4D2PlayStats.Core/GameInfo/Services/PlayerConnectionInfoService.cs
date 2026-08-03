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
    public async Task<List<PlayerConnectionInfoResult>> GetRelatedPlayersAsync(long communityId, CancellationToken cancellationToken = default)
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
                        FirstConnectedAtUtc = group.Min(connection => connection.FirstConnectedAtUtc),
                        LastConnectedAtUtc = player.LastConnectedAtUtc,
                        ConnectionCount = group.Sum(connection => connection.ConnectionCount)
                    };
                })
                .OrderByDescending(player => player.LastConnectedAtUtc)
        ];
    }

    public async Task AddOrUpdateAsync(PlayerConnectionInfoCommand command, CancellationToken cancellationToken = default)
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
            return;
        }

        entity.LastName = command.Name;
        entity.SteamId = steamIdentifiers.SteamId;
        entity.Steam3 = steamIdentifiers.Steam3;
        entity.ProfileUrl = steamIdentifiers.ProfileUrl;
        entity.LastConnectedAtUtc = connectedAtUtc;
        entity.ConnectionCount++;

        await playerConnectionInfoRepository.UpdateAsync(entity, entity.ETag, cancellationToken);
    }
}