using System.Globalization;
using L4D2PlayStats.Patents;
using L4D2PlayStats.Sdk.Ranking.Results;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Models;

public class RankingModel(IStringLocalizer<SharedResource> sharedLocalizer, PlayerResult player, PatentProgress patentProgress)
{
    public PlayerResult Player { get; } = player;
    public PatentProgressModel PatentProgress { get; } = new(sharedLocalizer, player, patentProgress);

    public class PatentProgressModel(IStringLocalizer<SharedResource> sharedLocalizer, PlayerResult player, PatentProgress patentProgress)
    {
        public Patent Patent => patentProgress.Patent;
        public Patent? NextPatent => patentProgress.NextPatent;

        public bool MaxLevel => patentProgress.MaxLevel;
        public bool LevelUp => patentProgress.LevelUp;
        public bool LevelDrop => patentProgress.LevelDrop;

        public decimal Progress => patentProgress.Progress;
        public decimal ProgressDifference => patentProgress.ProgressDifference;

        public decimal? ExperienceDifference => player.ExperienceDifference;

        public string ProgressBarType => LevelUp ? "success" : LevelDrop ? "danger" : "info";

        public string ProgressBarWidth
        {
            get
            {
                var progress = Progress;

                if (ProgressDifference != 0 && player.ExperienceDifference is > 0)
                    progress -= ProgressDifference;

                return $"{Math.Round(progress * 100, 2).ToString(CultureInfo.InvariantCulture)}%";
            }
        }

        public string PreviousExperienceProgressBarType => ExperienceDifference == null ? "info" : ExperienceDifference.Value > 0 ? "success" : "danger";
        public string PreviousExperienceProgressBarWidth => $"{Math.Round(ProgressDifference * 100, 2).ToString(CultureInfo.InvariantCulture)}%";

        public string Tooltip => string.Join("<br/>", TooltipMessages());

        private IEnumerable<string> TooltipMessages()
        {
            if (LevelUp)
                yield return sharedLocalizer["LevelUp"];

            if (LevelDrop)
                yield return sharedLocalizer["LevelDrop"];

            if (MaxLevel)
            {
                yield return sharedLocalizer["MaxPatentReached"];
                yield break;
            }

            if (ExperienceDifference is > 0)
                yield return string.Format(sharedLocalizer["LastGamePositiveXp"], ExperienceDifference.Value);

            if (ExperienceDifference is < 0)
                yield return string.Format(sharedLocalizer["LastGameNegativeXp"], ExperienceDifference.Value);

            if (NextPatent != null)
            {
                yield return string.Format(sharedLocalizer["NextLevel"], NextPatent.Level);
                yield return string.Format(sharedLocalizer["XpProgress"], player.Experience, NextPatent.Experience);
                yield return string.Format(sharedLocalizer["ProgressPercentage"], Progress);
            }
        }
    }
}