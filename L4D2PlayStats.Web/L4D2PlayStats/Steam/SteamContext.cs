using System.Text.Json;
using L4D2PlayStats.Steam.ServerInfo.Services;
using L4D2PlayStats.Steam.SteamUser.Services;
using Refit;

namespace L4D2PlayStats.Steam;

public class SteamContext : ISteamContext
{
    private const string BaseUrl = "https://api.steampowered.com";

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(5),
        BaseAddress = new Uri(BaseUrl)
    };

    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(Options)
    };

    public ISteamUserService SteamUserService => CreateService<ISteamUserService>();
    public IServerInfoService ServerInfoService => CreateService<IServerInfoService>();

    private static T CreateService<T>()
    {
        return RestService.For<T>(HttpClient, Settings);
    }
}