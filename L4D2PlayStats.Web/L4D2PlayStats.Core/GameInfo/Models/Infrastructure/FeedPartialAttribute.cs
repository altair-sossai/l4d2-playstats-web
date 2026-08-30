namespace L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

[AttributeUsage(AttributeTargets.Class)]
public sealed class FeedPartialAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}