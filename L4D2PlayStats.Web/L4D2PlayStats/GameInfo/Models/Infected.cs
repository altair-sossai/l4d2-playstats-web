using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class Infected : Player
{
    public InfectedType Type { get; set; }
    public int DamageTotal { get; set; }
    public int HealthPoints { get; set; }
    public int MaxHealthPoints { get; set; }
    public bool Ghost { get; set; }
    public bool Dead { get; set; }
    public bool Alive { get; set; }
    public bool IsBot { get; set; }
}