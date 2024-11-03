using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class Infected : Player
{
    public InfectedType Type { get; set; }
    public int DamageTotal { get; set; }
    public int HealthPoints { get; set; }
    public int MaxHealthPoints { get; set; }
}