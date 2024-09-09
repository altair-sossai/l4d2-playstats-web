using System.Reflection;
using L4D2PlayStats.Infrastructure.Attributes;

namespace L4D2PlayStats.Infrastructure.Extensions;

public static class EnumExtensions
{
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
}