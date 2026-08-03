namespace L4D2PlayStats.Core.GameInfo.Results;

public class PlayerConnectionInfoResult
{
    public required string CommunityId { get; set; } = string.Empty;
    public required string Name { get; set; } = string.Empty;
    public required string? SteamId { get; set; }
    public required string? Steam3 { get; set; }
    public required string? ProfileUrl { get; set; }
    public required DateTimeOffset FirstConnectedAtUtc { get; set; }
    public required DateTimeOffset LastConnectedAtUtc { get; set; }
    public required long ConnectionCount { get; set; }
}