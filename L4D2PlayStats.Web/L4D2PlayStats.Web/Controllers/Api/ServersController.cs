using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using L4D2PlayStats.Core.Steam.ServerInfo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using static L4D2PlayStats.Core.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/servers")]
[ApiController]
public class ServersController(
    IOptions<AppOptions> config,
    IMemoryCache memoryCache,
    IServerInfoService serverInfoService) : ControllerBase
{
    private readonly AsyncRetryPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)), (exception, _, retryCount, _) => { Console.WriteLine($"Retry {retryCount} due to: {exception.Message}"); });

    private string[] ServerIPs => config.Value.ServerIPs?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    private string SteamApiKey => config.Value.SteamApiKey!;

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var servers = await memoryCache.GetOrCreateAsync("Api.Servers", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            return GetServersInfo();
        });

        return Ok(servers);
    }

    private async Task<List<ServerInfo>> GetServersInfo()
    {
        var servers = new List<ServerInfo>();

        foreach (var serverIp in ServerIPs)
        {
            var serverInfo = await GetServerInfo(serverIp);

            if (serverInfo.Response?.Servers == null)
                continue;

            servers.AddRange(serverInfo.Response.Servers!);
        }

        return servers;
    }

    private Task<GetServerListResponse> GetServerInfo(string serverIp)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];

        return _retryPolicy.ExecuteAsync(() => serverInfoService.GetServerInfo(SteamApiKey, $"addr\\{ip}:{port}"));
    }
}