using System.ComponentModel;

namespace L4D2PlayStats.Core.Infrastructure.Extensions;

public static class EnumExtensions
{
    private static readonly Type DescriptionAttributeType = typeof(DescriptionAttribute);

    public static string Description(this Enum @enum)
    {
        var type = @enum.GetType();
        var fieldInfo = type.GetField(@enum.ToString());

        if (fieldInfo?.GetCustomAttributes(DescriptionAttributeType, false) is not DescriptionAttribute[] { Length: > 0 } descriptionAttributes)
            return @enum.ToString();

        var descriptionAttribute = descriptionAttributes.First();

        return descriptionAttribute.Description;
    }
}