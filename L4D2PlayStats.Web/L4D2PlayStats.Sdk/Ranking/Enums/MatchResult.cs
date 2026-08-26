using System.Text.Json.Serialization;

namespace L4D2PlayStats.Sdk.Ranking.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<MatchResult>))]
public enum MatchResult
{
    [JsonStringEnumMemberName("L")]
    Loss,

    [JsonStringEnumMemberName("D")]
    Draw,

    [JsonStringEnumMemberName("W")]
    Win
}