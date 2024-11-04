namespace L4D2PlayStats.GameInfo.Commands;

public class GameInfoCommand
{
    public int TeamSize { get; set; }
    public string? ConfigurationName { get; set; }
    public int AreTeamsFlipped { get; set; }
    public int MaxChapterProgressPoints { get; set; }
    public int SurvivorScore { get; set; }
    public int InfectedScore { get; set; }
}