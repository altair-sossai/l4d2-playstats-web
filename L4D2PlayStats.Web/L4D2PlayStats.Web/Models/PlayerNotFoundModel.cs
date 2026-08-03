using L4D2PlayStats.Core.GameInfo.Results;
using L4D2PlayStats.Core.Infrastructure.Structures;

namespace L4D2PlayStats.Web.Models;

public class PlayerNotFoundModel(long communityId, PlayerConnectionInfoResult? player, List<PlayerConnectionInfoResult> relatedPlayers)
{
    private readonly SteamIdentifiers _steamIdentifiers = GetSteamIdentifiers(communityId);

    public long CommunityId { get; } = communityId;
    public string? Name { get; } = player?.Name;
    public string? SteamId => player?.SteamId ?? _steamIdentifiers.SteamId;
    public string? ProfileUrl => player?.ProfileUrl ?? _steamIdentifiers.ProfileUrl;
    public DateTime? LastConnectedAtUtc { get; } = player?.LastConnectedAtUtc;
    public long? ConnectionCount { get; } = player?.ConnectionCount;
    public List<PlayerConnectionInfoResult> RelatedPlayers { get; } = relatedPlayers;

    private static SteamIdentifiers GetSteamIdentifiers(long communityId)
    {
        SteamIdentifiers.TryParse(communityId.ToString(), out var steamIdentifiers);

        return steamIdentifiers;
    }
}