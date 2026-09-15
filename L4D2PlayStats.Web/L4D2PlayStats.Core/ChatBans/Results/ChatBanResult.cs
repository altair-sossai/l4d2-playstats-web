using L4D2PlayStats.Core.ChatBans.Models;
using L4D2PlayStats.Core.UserAvatar;

namespace L4D2PlayStats.Core.ChatBans.Results;

public class ChatBanResult(ChatBanEntity entity)
{
    public string CommunityId { get; } = entity.RowKey;
    public string? SteamId { get; } = entity.SteamId;
    public string? Steam3 { get; } = entity.Steam3;
    public string? ProfileUrl { get; } = entity.ProfileUrl;
    public string Name { get; } = entity.LastName;
    public string? Reason { get; } = entity.Reason;
    public DateTime CreatedAtUtc { get; } = entity.CreatedAtUtc.UtcDateTime;
    public DateTime? UpdatedAtUtc { get; } = entity.UpdatedAtUtc?.UtcDateTime;
    public string? CreatedByName { get; } = entity.CreatedByName;
    public string? AvatarUrl { get; private set; }

    public void UpdateAvatarUrl(IUserAvatar userAvatar)
    {
        AvatarUrl = userAvatar[CommunityId];
    }
}