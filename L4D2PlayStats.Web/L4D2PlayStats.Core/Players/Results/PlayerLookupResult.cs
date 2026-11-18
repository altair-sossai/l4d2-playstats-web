namespace L4D2PlayStats.Core.Players.Results;

public class PlayerLookupResult
{
    public required string CommunityId { get; init; }
    public string? SteamId { get; init; }
    public string? Steam3 { get; init; }
    public string? ProfileUrl { get; init; }
    public string? Name { get; init; }
    public string? AvatarUrl { get; init; }
    public bool KnownOnSite { get; init; }
}
