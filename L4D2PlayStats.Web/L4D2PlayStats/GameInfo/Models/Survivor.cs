using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Models;

public class Survivor : Player
{
    public SurvivorCharacter Character { get; set; }
    public int PermanentHealth { get; set; }
    public int TemporaryHealth { get; set; }
    public Weapon PrimaryWeapon { get; set; }
    public Weapon SecondaryWeapon { get; set; }
    public MeleeWeapon? MeleeWeapon { get; set; }
    public Weapon SlotNumber3 { get; set; }
    public Weapon SlotNumber4 { get; set; }
    public Weapon SlotNumber5 { get; set; }
    public bool BlackAndWhite { get; set; }
    public bool Incapacitated { get; set; }
    public bool IsPlayerAlive { get; set; }
    public float Progress { get; set; }
}