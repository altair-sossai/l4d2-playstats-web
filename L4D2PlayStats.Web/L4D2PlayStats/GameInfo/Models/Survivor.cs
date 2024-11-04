using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class Survivor : Player
{
    public SurvivorCharacter Character { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public Weapon PrimaryWeapon { get; set; }
    public Weapon SecondaryWeapon { get; set; }
    public Weapon SlotNumber3 { get; set; }
    public Weapon SlotNumber4 { get; set; }
    public Weapon SlotNumber5 { get; set; }
    public MeleeWeapon? MeleeWeapon { get; set; }
    public bool BlackAndWhite { get; set; }
    public bool Incapacitated { get; set; }
    public bool Dead { get; set; }
    public bool Alive => !Dead;
    public bool IsBot { get; set; }
    public int Progress { get; set; }
}