namespace L4D2PlayStats.Core.AntiCheatAccess.Results;

public class AntiCheatAccessServerResult
{
    public string? Mode { get; init; }
    public List<ServerPlayer> Whitelist { get; init; } = [];
    public List<ServerPlayer> Required { get; init; } = [];

    public class ServerPlayer(AntiCheatAccessPlayerResult player)
    {
        public long AccountId { get; } = player.AccountId;
        public string CommunityId { get; } = player.CommunityId;
        public string Name { get; } = player.Name;
    }
}
