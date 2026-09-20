using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.AntiCheat.Responses;

public class ClientVersionResponse
{
    [JsonPropertyName("version")]
    public string? Version { get; set; }
}