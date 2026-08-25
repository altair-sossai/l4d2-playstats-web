using L4D2PlayStats.Core.Infrastructure.Networking;
using Microsoft.Extensions.Options;

namespace L4D2PlayStats.Core.Infrastructure.Options;

public class AppOptionsWraper(IOptions<AppOptions> config, IDnsResolver dnsResolver) : IAppOptionsWraper
{
    private static string[]? _steamApiKeys;
    private static int _steamApiKeysIndex;
    private static string[]? _serverAdmins;

    public string ServerId
    {
        get
        {
            if (string.IsNullOrEmpty(config.Value.ServerId))
                throw new InvalidOperationException("ServerId is not configured in AppOptions.");

            return config.Value.ServerId;
        }
    }

    public string SteamApiKey
    {
        get
        {
            if (string.IsNullOrEmpty(config.Value.SteamApiKey))
                throw new InvalidOperationException("SteamApiKey is not configured in AppOptions.");

            _steamApiKeys ??= Split(config.Value.SteamApiKey);

            if (_steamApiKeys.Length == 0)
                throw new InvalidOperationException("No valid SteamApiKey found in AppOptions.");

            if (_steamApiKeysIndex >= _steamApiKeys.Length)
                _steamApiKeysIndex = 0;

            return _steamApiKeys[_steamApiKeysIndex++];
        }
    }

    public string ServerDns => config.Value.ServerDns!.Trim();

    public string ServerIp => dnsResolver.Resolve(ServerDns);

    public string[] ServerAdmins
    {
        get
        {
            if (string.IsNullOrEmpty(config.Value.ServerAdmins))
                throw new InvalidOperationException("ServerAdmins is not configured in AppOptions.");

            _serverAdmins ??= Split(config.Value.ServerAdmins);

            if (_serverAdmins.Length == 0)
                throw new InvalidOperationException("No valid ServerAdmins found in AppOptions.");

            return _serverAdmins;
        }
    }

    private static string[] Split(string value)
    {
        if (string.IsNullOrEmpty(value))
            return [];

        return
        [
            .. value
                .Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries)
                .Select(v => v.Trim())
                .Where(v => !string.IsNullOrEmpty(v))
        ];
    }
}