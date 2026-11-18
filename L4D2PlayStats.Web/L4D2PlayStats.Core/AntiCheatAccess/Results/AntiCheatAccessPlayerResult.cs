using L4D2PlayStats.Core.AntiCheatAccess.Models;
using L4D2PlayStats.Core.UserAvatar;

namespace L4D2PlayStats.Core.AntiCheatAccess.Results;

public class AntiCheatAccessPlayerResult(AntiCheatAccessPlayerEntity entity)
{
    private const long CommunityIdBase = 76561197960265728;

    public string CommunityId { get; } = entity.RowKey;
    public long AccountId { get; } = long.TryParse(entity.RowKey, out var communityId) ? communityId - CommunityIdBase : 0;
    public string? SteamId { get; } = entity.SteamId;
    public string? Steam3 { get; } = entity.Steam3;
    public string? ProfileUrl { get; } = entity.ProfileUrl;
    public string Name { get; } = string.IsNullOrWhiteSpace(entity.LastName) ? entity.RowKey : entity.LastName;
    public string? AddedByName { get; } = entity.AddedByName;
    public string? AddedFrom { get; } = entity.AddedFrom;
    public DateTime CreatedAtUtc { get; } = entity.CreatedAtUtc.UtcDateTime;
    public string? AvatarUrl { get; private set; }

    public void UpdateAvatarUrl(IUserAvatar userAvatar)
    {
        AvatarUrl = userAvatar[CommunityId];
    }
}
