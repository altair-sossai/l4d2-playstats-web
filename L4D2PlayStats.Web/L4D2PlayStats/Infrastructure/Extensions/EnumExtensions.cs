using System.ComponentModel;
using System.Reflection;
using L4D2PlayStats.Infrastructure.Attributes;

namespace L4D2PlayStats.Infrastructure.Extensions;

public static class EnumExtensions
{
    private static readonly Type DescriptionAttributeType = typeof(DescriptionAttribute);

    public static string GetResourceKey(this Enum @enum)
    {
        var type = @enum.GetType();
        var value = @enum.ToString();
        var fieldInfo = type.GetField(value);

        if (fieldInfo == null)
            return value;

        var resourceKeyAttribute = fieldInfo.GetCustomAttribute<ResourceKeyAttribute>();

        return resourceKeyAttribute != null ? resourceKeyAttribute.Key : value;
    }

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