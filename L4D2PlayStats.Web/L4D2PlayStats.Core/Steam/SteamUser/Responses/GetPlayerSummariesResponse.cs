using System.Text.Json.Serialization;

namespace L4D2PlayStats.Core.Steam.SteamUser.Responses;

public class GetPlayerSummariesResponse
{
    [JsonPropertyName("response")]
    public ResponseInfo? Response { get; set; }

    public class ResponseInfo
    {
        [JsonPropertyName("players")]
        public List<PlayerInfo>? Players { get; set; }
    }

    public class PlayerInfo
    {
        [JsonPropertyName("steamid")]
        public string? SteamId { get; set; }

        [JsonPropertyName("personaname")]
        public string? PersonaName { get; set; }

        [JsonPropertyName("profileurl")]
        public string? ProfileUrl { get; set; }

        [JsonPropertyName("avatar")]
        public string? Avatar { get; set; }

        [JsonPropertyName("avatarmedium")]
        public string? AvatarMedium { get; set; }

        [JsonPropertyName("avatarfull")]
        public string? AvatarFull { get; set; }
    }
}