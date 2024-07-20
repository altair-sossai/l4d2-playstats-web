using L4D2PlayStats.Patent;
using L4D2PlayStats.Sdk.Ranking.Results;
using static L4D2PlayStats.Patent.PatentSystem;

namespace L4D2PlayStats.Web.Models;

public class PatentInfoModel(PlayerResult player)
{
    public PatentSystem.Patent Patent => GetPatent(player.Experience);
    public PatentSystem.Patent? PreviousExperiencePatent => player.PreviousExperience != null ? GetPatent(player.PreviousExperience.Value) : null;
    public PatentSystem.Patent? NextPatent => GetNextPatent(player.Experience);

    public bool LevelUp => PreviousExperiencePatent != null && Patent.Level > PreviousExperiencePatent.Level;
    public bool LevelDown => PreviousExperiencePatent != null && Patent.Level < PreviousExperiencePatent.Level;
    public decimal? ExperienceGain => player.PreviousExperience != null ? player.Experience - player.PreviousExperience : null;

    public bool MaxLevel => NextPatent == null;
    public decimal CurrentExperience => MaxLevel ? Patent.Experience : player.Experience;
    public decimal NextPatentExperience => NextPatent?.Experience ?? Patent.Experience;

    public decimal Progress
    {
        get
        {
            if (NextPatent == null)
                return 1;

            var current = CurrentExperience - Patent.Experience;
            var required = NextPatent.Experience - Patent.Experience;

            return current / required;
        }
    }

    public bool ShowPreviousExperienceProgressBar => player.PreviousExperience != null && !LevelUp && !LevelDown;

    public string ProgressBarType => LevelUp ? "success" : LevelDown ? "danger" : "info";

    public string PreviousExperienceProgressBarType => ExperienceGain switch
    {
        null => "info",
        > 0 => "success",
        < 0 => "danger",
        _ => "info"
    };

    public decimal ProgressBarWidth
    {
        get
        {
            if (!ShowPreviousExperienceProgressBar || NextPatent == null || ExperienceGain == null)
                return Math.Round(Progress * 100, 2);

            var current = CurrentExperience - Patent.Experience - ExperienceGain.Value;
            var required = NextPatent.Experience - Patent.Experience;

            return Math.Round(current / required * 100, 2);
        }
    }

    public decimal PreviousExperienceProgressBarWidth
    {
        get
        {
            if (!ShowPreviousExperienceProgressBar || NextPatent == null || ExperienceGain == null)
                return Math.Round(Progress * 100, 2);

            var current = Math.Abs(ExperienceGain.Value);
            var required = NextPatent.Experience - Patent.Experience;

            return Math.Round(current / required * 100, 2);
        }
    }

    public string Tooltip => string.Join("<br/>", TooltipMessages());

    private IEnumerable<string> TooltipMessages()
    {
        if (LevelUp)
            yield return "Level Up!";

        if (LevelDown)
            yield return "Level Down!";

        if (NextPatent == null)
        {
            yield return "Max patent reached";
            yield break;
        }

        if (ExperienceGain is > 0)
            yield return $"Last game: +{ExperienceGain:N0} xp";

        if (ExperienceGain is < 0)
            yield return $"Last game: {ExperienceGain.Value:N0} xp";

        yield return $"Next Level: {NextPatent.Level}";
        yield return $"XP: {CurrentExperience:N0} / {NextPatentExperience:N0}";
        yield return $"Progress: {Progress:P0}";
    }
}