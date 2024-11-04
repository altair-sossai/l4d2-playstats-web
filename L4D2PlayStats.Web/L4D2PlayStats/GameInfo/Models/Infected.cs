using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class Infected : Player
{
    public InfectedType Type { get; set; }
    public int DamageTotal { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public bool Ghost { get; set; }
    public bool Dead { get; set; }
    public bool Alive => !Dead;
    public bool IsBot { get; set; }
}