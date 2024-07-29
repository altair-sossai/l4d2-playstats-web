using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Patents.Services;

public class PatentService : IPatentService
{
    private static readonly List<Patent> Patents =
    [
        new Patent(1, "Bronze #1"),
        new Patent(2, "Bronze #2"),
        new Patent(3, "Bronze #3"),
        new Patent(4, "Bronze #4"),
        new Patent(5, "Bronze #5"),
        new Patent(6, "Silver #1"),
        new Patent(7, "Silver #2"),
        new Patent(8, "Silver #3"),
        new Patent(9, "Silver #4"),
        new Patent(10, "Silver #5"),
        new Patent(11, "Gold #1"),
        new Patent(12, "Gold #2"),
        new Patent(13, "Gold #3"),
        new Patent(14, "Gold #4"),
        new Patent(15, "Global")
    ];

    public IReadOnlyCollection<Patent> GetAllPatents()
    {
        return Patents;
    }

    public Patent GetPatent(decimal experience)
    {
        for (var i = Patents.Count - 1; i >= 0; i--)
            if (experience >= Patents[i].Experience)
                return Patents[i];

        return Patents[0];
    }

    public Patent? GetNextPatent(decimal experience)
    {
        var current = GetPatent(experience);

        return Patents.FirstOrDefault(p => p.Level > current.Level);
    }

    public PatentProgress GetPatentProgress(PlayerResult player)
    {
        return new PatentProgress(this, player);
    }
}