namespace L4D2PlayStats.Sdk.Ranking.Results;

public class PlayerRelationResult
{
    public long CommunityId { get; set; }

    public int TogetherWins { get; set; }
    public int TogetherLosses { get; set; }
    public int TogetherGames { get; set; }

    public int AgainstWins { get; set; }
    public int AgainstLosses { get; set; }
    public int AgainstGames { get; set; }
}