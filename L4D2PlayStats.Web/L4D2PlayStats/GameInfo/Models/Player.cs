using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo.Models;

public class Player
{
    private SteamIdentifiers _steamIdentifiers;

    public long CommunityId
    {
        get => _steamIdentifiers.CommunityId ?? 0;
        init => SteamIdentifiers.TryParse(value.ToString(), out _steamIdentifiers);
    }

    public string? SteamId => _steamIdentifiers.SteamId;
    public string? Steam3 => _steamIdentifiers.Steam3;
    public string? ProfileUrl => _steamIdentifiers.ProfileUrl;
    public string? Name { get; set; }
    public int Latency { get; set; }
}