using L4D2PlayStats.Patents.Configs;

namespace L4D2PlayStats.Patents;

public class Patent(int level, string name)
{
    public int Level { get; } = level;
    public string Name { get; } = name;
    public string FullName => $"Level {Level:00} - {Name}";

    public int Experience
    {
        get
        {
            var normalized = (Level - PatentConfig.MinLevel) / ((double)PatentConfig.MaxLevel - PatentConfig.MinLevel);
            var exponential = Math.Pow(normalized, 2);
            var result = PatentConfig.MinExperience + exponential * (PatentConfig.MaxExperience - PatentConfig.MinExperience);

            result = Math.Floor(result / 10) * 10;

            return (int)result;
        }
    }

    public string Image => $"/imgs/patents/{Level:00}.png";
}