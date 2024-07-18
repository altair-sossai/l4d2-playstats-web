using L4D2PlayStats.Patent;
using L4D2PlayStats.Sdk.Ranking.Results;
using static L4D2PlayStats.Patent.PatentSystem;

namespace L4D2PlayStats.Web.Models;

public class PatentProgressBarModel(PlayerResult player)
{
    public PatentSystem.Patent Patent => GetPatent(player.Experience);
    public PatentSystem.Patent? NextPatent => GetNextPatent(player.Experience);
    public bool MaxLevel => NextPatent == null;
    public decimal CurrentXp => MaxLevel ? Patent.Experience : player.Experience;
    public decimal NextXp => NextPatent?.Experience ?? Patent.Experience;

    public decimal Progress
    {
        get
        {
            if (NextPatent == null)
                return 1;

            var required = NextPatent.Experience - Patent.Experience;
            var current = CurrentXp - Patent.Experience;

            return current / required;
        }
    }

    public decimal Width => Math.Round(Progress * 100, 2);

    public string Tooltip => NextPatent == null
        ? "Max patent reached"
        : string.Join("<br/>",
            $"Next Level: {NextPatent.Level}",
            $"XP: {CurrentXp:N0} / {NextXp:N0}",
            $"Progress: {Progress:P0}");
}