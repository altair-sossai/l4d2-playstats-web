using L4D2PlayStats.Core.GameInfo.Enums;

namespace L4D2PlayStats.Core.GameInfo.Models;

public class Survivor : Player
{
    public SurvivorCharacter Character { get; set; }
    public int? PermanentHealth { get; set; }
    public int? TemporaryHealth { get; set; }
    public int Health => (PermanentHealth ?? 0) + (TemporaryHealth ?? 0);
    public int MaxHealth => Dead ? 0 : Incapacitated == true ? 300 : 100;

    public HealthColor? HealthColor
    {
        get
        {
            if (Incapacitated == true)
                return Enums.HealthColor.Red;

            return PermanentHealth switch
            {
                null => null,
                < 25 => Enums.HealthColor.Red,
                < 40 => Enums.HealthColor.Orange,
                _ => Enums.HealthColor.Green
            };
        }
    }

    public Weapon? PrimaryWeapon { get; set; }
    public Weapon? SecondaryWeapon { get; set; }
    public MeleeWeapon? MeleeWeapon { get; set; }
    public Weapon? SlotNumber3 { get; set; }
    public Weapon? SlotNumber4 { get; set; }
    public Weapon? SlotNumber5 { get; set; }
    public bool? BlackAndWhite { get; set; }
    public bool? Incapacitated { get; set; }
    public bool IsPlayerAlive { get; set; }
    public bool Dead => !IsPlayerAlive;
    public decimal Progress { get; set; }
}