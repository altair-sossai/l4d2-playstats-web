using L4D2PlayStats.Core.GameInfo.Models;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.UserAvatar;

namespace L4D2PlayStats.Core.ChatBans.Results;

public class RecentChatSenderResult(ExternalChatMessage message, bool alreadyBanned)
{
    public string CommunityId { get; } = message.CommunityId!;
    public string? SteamId { get; } = message.SteamId;
    public string? Steam3 { get; } = SteamIdentifiers.TryParse(message.CommunityId, out var steamIdentifiers) ? steamIdentifiers.Steam3 : null;
    public string? ProfileUrl { get; } = message.ProfileUrl;
    public string Name { get; } = message.Name ?? string.Empty;
    public string Message { get; } = message.Text ?? string.Empty;
    public DateTime When { get; } = message.When;
    public bool AlreadyBanned { get; } = alreadyBanned;
    public string? AvatarUrl { get; private set; }

    public void UpdateAvatarUrl(IUserAvatar userAvatar)
    {
        AvatarUrl = userAvatar[CommunityId];
    }
}