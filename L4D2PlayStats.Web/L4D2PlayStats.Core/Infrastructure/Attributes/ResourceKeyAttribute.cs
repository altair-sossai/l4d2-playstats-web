namespace L4D2PlayStats.Core.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class ResourceKeyAttribute(string key) : Attribute
{
    public string Key { get; } = key;
}