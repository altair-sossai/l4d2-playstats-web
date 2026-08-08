using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerRelationModel(PlayerRelationResult relation, PlayerResult? player)
{
    public long CommunityId { get; } = relation.CommunityId;
    public PlayerResult? Player { get; } = player;
    public string Name => Player?.Name ?? CommunityId.ToString();

    public int TogetherGames => relation.TogetherGames;
    public int TogetherWins => relation.TogetherWins;
    public int TogetherLosses => relation.TogetherLosses;
    public decimal TogetherWinRate => TogetherGames == 0 ? 0 : TogetherWins / (decimal)TogetherGames;
    public decimal TogetherLossRate => TogetherGames == 0 ? 0 : TogetherLosses / (decimal)TogetherGames;

    public int AgainstGames => relation.AgainstGames;
    public int AgainstWins => relation.AgainstWins;
    public int AgainstLosses => relation.AgainstLosses;
    public decimal AgainstWinRate => AgainstGames == 0 ? 0 : AgainstWins / (decimal)AgainstGames;
    public decimal AgainstLossRate => AgainstGames == 0 ? 0 : AgainstLosses / (decimal)AgainstGames;
}