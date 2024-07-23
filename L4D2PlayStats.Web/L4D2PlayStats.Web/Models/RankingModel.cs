using L4D2PlayStats.Patents;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class RankingModel(PlayerResult player, PatentProgress patentProgress)
{
    public PlayerResult Player { get; } = player;
    public PatentProgressModel PatentProgress { get; } = new(player, patentProgress);

    public class PatentProgressModel(PlayerResult player, PatentProgress patentProgress)
    {
        public Patent Patent => patentProgress.Patent;
        public Patent? NextPatent => patentProgress.NextPatent;

        public bool MaxLevel => patentProgress.MaxLevel;
        public bool LevelUp => patentProgress.LevelUp;
        public bool LevelDown => patentProgress.LevelDown;

        public decimal Progress => patentProgress.Progress;
        public decimal ProgressDifference => patentProgress.ProgressDifference;

        public decimal? ExperienceDifference => player.ExperienceDifference;

        public string ProgressBarType => LevelUp ? "success" : LevelDown ? "danger" : "info";

        public decimal ProgressBarWidth
        {
            get
            {
                var progress = Progress;

                if (ProgressDifference != 0 && player.ExperienceDifference is > 0)
                    progress -= ProgressDifference;

                return Math.Round(progress * 100, 2);
            }
        }

        public string PreviousExperienceProgressBarType => ExperienceDifference == null ? "info" : ExperienceDifference.Value > 0 ? "success" : "danger";
        public decimal PreviousExperienceProgressBarWidth => Math.Round(ProgressDifference * 100, 2);

        public string Tooltip => string.Join("<br/>", TooltipMessages());

        private IEnumerable<string> TooltipMessages()
        {
            if (LevelUp)
                yield return "Level Up!";

            if (LevelDown)
                yield return "Level Down!";

            if (MaxLevel)
            {
                yield return "Max patent reached";
                yield break;
            }

            if (ExperienceDifference is > 0)
                yield return $"Last game: +{ExperienceDifference:N0} xp";

            if (ExperienceDifference is < 0)
                yield return $"Last game: {ExperienceDifference.Value:N0} xp";

            if (NextPatent != null)
            {
                yield return $"Next Level: {NextPatent.Level}";
                yield return $"XP: {player.Experience:N0} / {NextPatent.Experience:N0}";
                yield return $"Progress: {Progress:P0}";
            }
        }
    }
}