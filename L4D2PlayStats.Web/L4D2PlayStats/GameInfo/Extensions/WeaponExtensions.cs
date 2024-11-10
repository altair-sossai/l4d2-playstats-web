using L4D2PlayStats.GameInfo.Attributes;
using L4D2PlayStats.GameInfo.Enums;

namespace L4D2PlayStats.GameInfo.Extensions;

public static class WeaponExtensions
{
    private static readonly Type WeaponType = typeof(Weapon);
    private static readonly Type WeaponNameAttributeType = typeof(WeaponNameAttribute);

    public static string? WeaponName(this Weapon weapon)
    {
        var fieldInfo = WeaponType.GetField(weapon.ToString());
        var attributes = fieldInfo?.GetCustomAttributes(WeaponNameAttributeType, false);
        var weaponNameAttribute = attributes?.FirstOrDefault() as WeaponNameAttribute;

        return weaponNameAttribute?.Name;
    }
}