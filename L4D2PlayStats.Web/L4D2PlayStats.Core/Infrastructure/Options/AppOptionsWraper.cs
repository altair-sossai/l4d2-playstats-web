using Microsoft.Extensions.Options;

namespace L4D2PlayStats.Core.Infrastructure.Options;

public class AppOptionsWraper(IOptions<AppOptions> config) : IAppOptionsWraper
{
    private static string[]? _steamApiKeys;
    private static int _steamApiKeysIndex;
    private static string[]? _serverIPs;
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

    public string[] ServerIps
    {
        get
        {
            if (string.IsNullOrEmpty(config.Value.ServerIPs))
                throw new InvalidOperationException("ServerIPs is not configured in AppOptions.");

            _serverIPs ??= Split(config.Value.ServerIPs);

            if (_serverIPs.Length == 0)
                throw new InvalidOperationException("No valid ServerIPs found in AppOptions.");

            return _serverIPs;
        }
    }

    public string PrimaryServerIp => ServerIps.First();

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

        return value
            .Split([',', ';', '|'], StringSplitOptions.RemoveEmptyEntries)
            .Select(v => v.Trim())
            .Where(v => !string.IsNullOrEmpty(v))
            .ToArray();
    }
}