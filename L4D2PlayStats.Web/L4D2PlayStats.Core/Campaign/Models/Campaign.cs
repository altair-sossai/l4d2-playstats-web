namespace L4D2PlayStats.Core.Campaign.Models;

public class Campaign(string name, List<string> maps)
{
    public string Name { get; set; } = name;
    public List<string> Maps { get; set; } = maps;
}