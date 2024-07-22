namespace L4D2PlayStats.Patent;

public static class PatentSystem
{
    private const int ExperienceByPatent = 100;
    private const double PatentExperienceMultiplier = 1.298;

    public static readonly List<Patent> Patents =
    [
        new Patent(1, "Survivor Recruit"),
        new Patent(2, "Rookie Rescuer"),
        new Patent(3, "Infected Hunter"),
        new Patent(4, "Safehouse Defender"),
        new Patent(5, "Urban Scout"),
        new Patent(6, "Quarantine Breaker"),
        new Patent(7, "Outbreak Specialist"),
        new Patent(8, "Zombie Exterminator"),
        new Patent(9, "Apocalypse Tracker"),
        new Patent(10, "Pandemic Protector"),
        new Patent(11, "Virus Vanquisher"),
        new Patent(12, "Survival Strategist"),
        new Patent(13, "Infection Commander"),
        new Patent(14, "Contagion Master"),
        new Patent(15, "Ultimate Survivor")
    ];

    public static Patent GetPatent(decimal experience)
    {
        for (var i = Patents.Count - 1; i >= 0; i--)
            if (experience >= Patents[i].Experience)
                return Patents[i];

        return Patents[0];
    }

    public static Patent? GetNextPatent(decimal experience)
    {
        var current = GetPatent(experience);

        return Patents.FirstOrDefault(p => p.Level > current.Level);
    }

    public class Patent(int level, string name)
    {
        public int Level { get; } = level;
        public string Name { get; } = name;
        public string FullName => $"Level {Level:00} - {Name}";
        public int Experience => Level == 1 ? 0 : (int)(Math.Floor(ExperienceByPatent * Math.Pow(PatentExperienceMultiplier, Level) / 50.0) * 50.0);
        public string Image => $"/imgs/patents/{Level}.png";
    }
}