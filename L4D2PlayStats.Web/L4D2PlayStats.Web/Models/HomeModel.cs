namespace L4D2PlayStats.Web.Models;

public class HomeModel(List<RankingModel> ranking)
{
    private readonly RankingPeriodModel _period = new();

    public int NextResetIn => _period.NextResetIn;
    public List<RankingModel> Ranking { get; } = ranking;
}