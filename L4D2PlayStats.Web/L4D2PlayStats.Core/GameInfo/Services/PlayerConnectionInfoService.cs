using System.Net;
using L4D2PlayStats.Core.GameInfo.Commands;
using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.GameInfo.Repositories;
using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Core.GameInfo.Services;

public class PlayerConnectionInfoService(IPlayerConnectionInfoRepository playerConnectionInfoRepository) : IPlayerConnectionInfoService
{
    public async Task AddOrUpdateAsync(PlayerConnectionInfoCommand command, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(command.CommunityId))
            throw new ArgumentException("Community ID must be provided.", nameof(command.CommunityId));

        if (string.IsNullOrWhiteSpace(command.IpAddress))
            throw new ArgumentException("IP address must be provided.", nameof(command.IpAddress));

        if (!IPAddress.TryParse(command.IpAddress, out var ipAddress))
            throw new ArgumentException("IP address is invalid.", nameof(command.IpAddress));

        if (string.IsNullOrWhiteSpace(command.Name))
            throw new ArgumentException("Name must be provided.", nameof(command.Name));

        SteamIdentifiers.TryParse(command.CommunityId, out var steamIdentifiers);

        var connectedAtUtc = DateTimeOffset.UtcNow;
        var entity = await playerConnectionInfoRepository.FindAsync(ipAddress.ToString(), command.CommunityId, cancellationToken);

        if (entity is null)
        {
            entity = new PlayerConnectionInfoEntity
            {
                PartitionKey = ipAddress.ToString(),
                RowKey = command.CommunityId,
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