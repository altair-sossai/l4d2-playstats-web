using System.Text.Json.Serialization;

namespace L4D2PlayStats.Steam.ServerInfo.Responses;

public class GetServerListResponse
{
    [JsonPropertyName("response")]
    public ServersInfo? Response { get; set; }

    public class ServersInfo
    {
        [JsonPropertyName("servers")]
        public List<ServerInfo?>? Servers { get; set; }
    }

    public class ServerInfo
    {
        private string? _gametype;

        [JsonPropertyName("addr")]
        public string? Addr { get; set; }

        [JsonPropertyName("gameport")]
        public int? Gameport { get; set; }

        [JsonPropertyName("steamid")]
        public string? Steamid { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("appid")]
        public int? Appid { get; set; }

        [JsonPropertyName("gamedir")]
        public string? Gamedir { get; set; }

        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("product")]
        public string? Product { get; set; }

        [JsonPropertyName("region")]
        public int? Region { get; set; }

        [JsonPropertyName("players")]
        public int? Players { get; set; }

        [JsonPropertyName("max_players")]
        public int? MaxPlayers { get; set; }

        [JsonPropertyName("bots")]
        public int? Bots { get; set; }

        [JsonPropertyName("map")]
        public string? Map { get; set; }

        [JsonPropertyName("secure")]
        public bool? Secure { get; set; }

        [JsonPropertyName("dedicated")]
        public bool? Dedicated { get; set; }

        [JsonPropertyName("os")]
        public string? Os { get; set; }

        [JsonPropertyName("gametype")]
        public string? Gametype
        {
            get => _gametype;
            set => _gametype = string.IsNullOrEmpty(value) ? value : string.Join(", ", value.Split(','));
        }
    }
}