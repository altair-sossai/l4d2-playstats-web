using FluentAssertions;
using L4D2PlayStats.Patents;
using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Tests.Patents;

[TestClass]
public class PatentProgressTests
{
    private readonly IPatentService _patentService = new PatentService();

    [TestMethod]
    public void Patents_Experience_0_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 0
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Patent.Level.Should().Be(1);
        progress.NextPatent!.Level.Should().Be(2);
        progress.PreviousExperiencePatent.Should().BeNull();

        progress.MaxLevel.Should().BeFalse();
        progress.LevelUp.Should().BeFalse();
        progress.LevelDown.Should().BeFalse();

        progress.Progress.Should().Be(0);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void Patents_Experience_100_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 100
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Patent.Level.Should().Be(3);
        progress.NextPatent!.Level.Should().Be(4);
        progress.PreviousExperiencePatent.Should().BeNull();

        progress.MaxLevel.Should().BeFalse();
        progress.LevelUp.Should().BeFalse();
        progress.LevelDown.Should().BeFalse();

        progress.Progress.Should().BeApproximately(0, 0.001m);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void Patents_Experience_200_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 200
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Patent.Level.Should().Be(3);
        progress.NextPatent!.Level.Should().Be(4);
        progress.PreviousExperiencePatent.Should().BeNull();

        progress.MaxLevel.Should().BeFalse();
        progress.LevelUp.Should().BeFalse();
        progress.LevelDown.Should().BeFalse();

        progress.Progress.Should().BeApproximately(0.833m, 0.001m);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void Patents_Experience_200_WithPreviousExperiencePositive_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 200,
            PreviousExperience = 190
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Patent.Level.Should().Be(3);
        progress.NextPatent!.Level.Should().Be(4);
        progress.PreviousExperiencePatent!.Level.Should().Be(3);

        progress.MaxLevel.Should().BeFalse();
        progress.LevelUp.Should().BeFalse();
        progress.LevelDown.Should().BeFalse();

        progress.Progress.Should().BeApproximately(0.833m, 0.001m);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void Patents_Experience_200_WithPreviousExperienceNegative_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 200,
            PreviousExperience = 260
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Patent.Level.Should().Be(3);
        progress.NextPatent!.Level.Should().Be(4);
        progress.PreviousExperiencePatent!.Level.Should().Be(4);

        progress.MaxLevel.Should().BeFalse();
        progress.LevelUp.Should().BeFalse();
        progress.LevelDown.Should().BeTrue();

        progress.Progress.Should().BeApproximately(0.833m, 0.001m);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void MaxLevel_4999_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 4999
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.MaxLevel.Should().BeFalse();
    }

    [TestMethod]
    public void MaxLevel_5000_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 5000
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.MaxLevel.Should().BeTrue();
    }

    [TestMethod]
    public void MaxLevel_9999_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 9999
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.MaxLevel.Should().BeTrue();
    }

    [TestMethod]
    public void Progress_Experience_50_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 50
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Progress.Should().BeApproximately(0.375m, 0.001m);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void Progress_Experience_585_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 585
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Progress.Should().BeApproximately(0.804m, 0.001m);
        progress.ProgressDifference.Should().Be(0);
    }

    [TestMethod]
    public void ProgressDifference_Positive_Test()
    {
        var playerResult = new PlayerResult
        {
            Experience = 100,
            ExperienceDifference = 25
        };

        var progress = new PatentProgress(_patentService, playerResult);

        progress.Progress.Should().Be(0);
        progress.ProgressDifference.Should().BeApproximately(0.208m, 0.001m);
    }
}