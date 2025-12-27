using L4D2PlayStats.Core.GameInfo.Enums;
using L4D2PlayStats.Core.Infrastructure.Structures;
using L4D2PlayStats.Core.UserAvatar;

namespace L4D2PlayStats.Core.GameInfo.Models;

public class Player
{
    private readonly SteamIdentifiers _steamIdentifiers;

    public string? CommunityId
    {
        get => _steamIdentifiers.CommunityId?.ToString();
        init => SteamIdentifiers.TryParse(value, out _steamIdentifiers);
    }

    public string? SteamId => _steamIdentifiers.SteamId;
    public string? Steam3 => _steamIdentifiers.Steam3;
    public string? ProfileUrl => _steamIdentifiers.ProfileUrl;
    public string? Name { get; set; }
    public bool IsAdmin { get; set; }
    public decimal? Latency { get; set; }

    public LatencyType? LatencyType => Latency switch
    {
        null => null,
        < 0.1m => Enums.LatencyType.Low,
        < 0.2m => Enums.LatencyType.Medium,
        _ => Enums.LatencyType.High
    };

    public string? AvatarUrl { get; private set; }

    public void UpdateAvatarUrl(IUserAvatar userAvatar)
    {
        AvatarUrl = userAvatar[CommunityId];
    }
}