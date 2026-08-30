using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.GameInfo.Models.Infrastructure;

public abstract class FeedItem
{
    private static readonly ConcurrentDictionary<Type, string> PartialNames = new();

    public DateTime When { get; set; } = DateTime.UtcNow;

    [JsonIgnore]
    public long Ticks => When.Ticks;

    [JsonIgnore]
    public string PartialName
    {
        get
        {
            var type = GetType();

            return PartialNames.GetOrAdd(type, static type =>
            {
                var attribute = type.GetCustomAttribute<FeedPartialAttribute>();

                if (string.IsNullOrWhiteSpace(attribute?.Name))
                    throw new InvalidOperationException($"{type.Name} is missing a [FeedPartial] attribute.");

                return attribute.Name;
            });
        }
    }
}