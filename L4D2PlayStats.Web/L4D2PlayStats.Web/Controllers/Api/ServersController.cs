﻿using L4D2PlayStats.Core.Steam.ServerInfo.Responses;
using L4D2PlayStats.Core.Steam.ServerInfo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using static L4D2PlayStats.Core.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Controllers.Api;

[Route("api/servers")]
[ApiController]
public class ServersController(
    IConfiguration configuration,
    IMemoryCache memoryCache,
    IServerInfoService serverInfoService) : ControllerBase
{
    private string[] ServerIPs => configuration.GetValue<string>("ServerIPs")?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? [];
    private string SteamApiKey => configuration.GetValue<string>("SteamApiKey")!;

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

        return serverInfoService.GetServerInfo(SteamApiKey, $"addr\\{ip}:{port}");
    }
}