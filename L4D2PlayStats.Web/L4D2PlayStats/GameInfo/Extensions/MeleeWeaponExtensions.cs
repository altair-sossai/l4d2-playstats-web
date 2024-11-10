using L4D2PlayStats.GameInfo.Attributes;
using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Extensions;

public static class MeleeWeaponExtensions
{
    private static readonly Type MeleeWeaponType = typeof(MeleeWeapon);
    private static readonly Type WeaponNameAttributeType = typeof(WeaponNameAttribute);

    public static string? WeaponName(this MeleeWeapon meleeWeapon)
    {
        var fieldInfo = MeleeWeaponType.GetField(meleeWeapon.ToString());
        var attributes = fieldInfo?.GetCustomAttributes(WeaponNameAttributeType, false);
        var weaponNameAttribute = attributes?.FirstOrDefault() as WeaponNameAttribute;

        return weaponNameAttribute?.Name;
    }
}