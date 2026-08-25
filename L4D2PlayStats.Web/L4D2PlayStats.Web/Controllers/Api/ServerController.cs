using L4D2PlayStats.Core.Infrastructure.Options;
using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using L4D2PlayStats.Core.Steam.ServerInfo.Services.Cache;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static L4D2PlayStats.Core.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Controllers.Api;

[ApiController]
public class ServerController(
    IAppOptionsWraper config,
    IMemoryCache memoryCache,
    IServerInfoServiceCached serverInfoService) : ControllerBase
{
    [HttpGet("api/server")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var server = await memoryCache.GetOrCreateAsync("Api.Server", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);

            return GetServerInfo(cancellationToken);
        });

        return Ok(server);
    }

    [HttpGet("api/servers")]
    public IActionResult GetLegacy()
    {
        return RedirectPermanent("/api/server");
    }

    private async Task<ServerInfo?> GetServerInfo(CancellationToken cancellationToken)
    {
        var serverList = await GetServerList(config.ServerIp, cancellationToken);

        return serverList?.Response?.Servers?.OfType<ServerInfo>().FirstOrDefault();
    }

    private Task<GetServerListResponse?> GetServerList(string serverIp, CancellationToken cancellationToken)
    {
        var segments = serverIp.Split(':');

        if (segments.Length != 2)
            throw new ArgumentException("Invalid server IP format");

        if (!int.TryParse(segments[1], out var port))
            throw new ArgumentException("Invalid server port");

        var ip = segments[0];

        return serverInfoService.GetServerInfoAsync(config.SteamApiKey, $"addr\\{ip}:{port}", cancellationToken);
    }
}