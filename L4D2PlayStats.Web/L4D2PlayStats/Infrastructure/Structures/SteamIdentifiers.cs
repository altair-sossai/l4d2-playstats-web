using L4D2PlayStats.Infrastructure.Helpers;

namespace L4D2PlayStats.Infrastructure.Structures;

public readonly struct SteamIdentifiers
{
    private SteamIdentifiers(long? communityId)
    {
        CommunityId = communityId;
    }

    public long? CommunityId { get; }
    public string? SteamId => SteamIdHelper.CommunityIdToSteamId(CommunityId ?? 0);
    public string? Steam3 => SteamIdHelper.CommunityIdToSteam3(CommunityId ?? 0);
    public string? ProfileUrl => SteamIdHelper.ProfileUrl(CommunityId ?? 0);

    private bool HasValue => CommunityId is > 0;

    public static bool TryParse(string? value, out SteamIdentifiers steamIdentifiers)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            steamIdentifiers = new SteamIdentifiers(null);
            return false;
        }

        var communityId = SteamIdHelper.SteamIdToCommunityId(value)
                          ?? SteamIdHelper.Steam3ToCommunityId(value)
                          ?? SteamIdHelper.ParseCommunityId(value);

        steamIdentifiers = new SteamIdentifiers(communityId);

        return steamIdentifiers.HasValue;
    }
}