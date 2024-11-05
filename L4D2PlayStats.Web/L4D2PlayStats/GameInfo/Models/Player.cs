using L4D2PlayStats.Infrastructure.Structures;

namespace L4D2PlayStats.GameInfo.Models;

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
    public decimal Latency { get; set; }
}