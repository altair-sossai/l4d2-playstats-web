using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class Infected : Player
{
    public InfectedType Type { get; set; }
    public int Damage { get; set; }
    public int Health { get; set; }
    public int MaxHealth { get; set; }
    public bool IsInfectedGhost { get; set; }
    public bool IsPlayerAlive { get; set; }
}