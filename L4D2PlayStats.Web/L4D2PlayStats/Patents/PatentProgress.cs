using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Patents;

public class PatentProgress
{
    private readonly PlayerResult _player;

    public PatentProgress(IPatentService patentService, PlayerResult player)
    {
        _player = player;

        Patent = patentService.GetPatent(player.Experience);
        NextPatent = patentService.GetNextPatent(player.Experience);

        if (player.PreviousExperience != null)
            PreviousExperiencePatent = patentService.GetPatent(player.PreviousExperience.Value);
    }

    public Patent Patent { get; }
    public Patent? NextPatent { get; }
    public Patent? PreviousExperiencePatent { get; }

    public bool MaxLevel => NextPatent == null;
    public bool LevelUp => PreviousExperiencePatent != null && Patent.Level > PreviousExperiencePatent.Level;
    public bool LevelDown => PreviousExperiencePatent != null && Patent.Level < PreviousExperiencePatent.Level;

    public decimal Progress
    {
        get
        {
            if (MaxLevel)
                return 1;

            var current = _player.Experience - Patent.Experience;
            var required = NextPatent!.Experience - Patent.Experience;

            return current / required;
        }
    }

    public decimal ProgressDifference
    {
        get
        {
            if (MaxLevel || LevelUp || LevelDown || _player.ExperienceDifference == null)
                return 0;

            var current = Math.Abs(_player.ExperienceDifference.Value);
            var required = NextPatent!.Experience - Patent.Experience;

            return current / required;
        }
    }
}