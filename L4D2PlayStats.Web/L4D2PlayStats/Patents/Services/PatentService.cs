using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Patents.Services;

public class PatentService : IPatentService
{
    private static readonly List<Patent> Patents =
    [
        new(1, "Bronze #1"),
        new(2, "Bronze #2"),
        new(3, "Bronze #3"),
        new(4, "Bronze #4"),
        new(5, "Bronze #5"),
        new(6, "Silver #1"),
        new(7, "Silver #2"),
        new(8, "Silver #3"),
        new(9, "Silver #4"),
        new(10, "Silver #5"),
        new(11, "Gold #1"),
        new(12, "Gold #2"),
        new(13, "Gold #3"),
        new(14, "Gold #4"),
        new(15, "Global")
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