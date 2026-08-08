using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerRelationRowModel(long communityId, PlayerResult? player, PlayerRelationResult? first, PlayerRelationResult? second)
{
    public long CommunityId { get; } = communityId;
    public PlayerResult? Player { get; } = player;
    public string Name => Player?.Name ?? CommunityId.ToString();

    public PlayerRelationModel? First { get; } = first == null ? null : new PlayerRelationModel(first, player);
    public PlayerRelationModel? Second { get; } = second == null ? null : new PlayerRelationModel(second, player);

    public int TogetherGames => (First?.TogetherGames ?? 0) + (Second?.TogetherGames ?? 0);
    public int TogetherWins => (First?.TogetherWins ?? 0) + (Second?.TogetherWins ?? 0);

    public int AgainstGames => (First?.AgainstGames ?? 0) + (Second?.AgainstGames ?? 0);
    public int AgainstWins => (First?.AgainstWins ?? 0) + (Second?.AgainstWins ?? 0);

    public int TotalGames => First != null
        ? First.TogetherGames + First.AgainstGames
        : (Second?.TogetherGames ?? 0) + (Second?.AgainstGames ?? 0);
}