﻿using L4D2PlayStats.Steam.Players;
using L4D2PlayStats.Steam.ServerInfo.Responses;
using static L4D2PlayStats.Steam.ServerInfo.Responses.GetServerListResponse;

namespace L4D2PlayStats.Web.Models;

public class ServerInfoModel(string serverIp, GetServerListResponse? serverInfoResponse, List<Player>? players)
{
    public string ServerIp { get; } = serverIp;
    public ServerInfo? ServersInfo { get; } = serverInfoResponse?.Response?.Servers?.FirstOrDefault();
    public List<Player>? Players { get; } = players;
}