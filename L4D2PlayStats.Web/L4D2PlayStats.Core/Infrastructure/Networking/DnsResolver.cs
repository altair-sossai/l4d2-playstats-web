using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace L4D2PlayStats.Core.Infrastructure.Networking;

public class DnsResolver : IDnsResolver
{
    private const int DefaultServerPort = 27015;

    private static readonly ConcurrentDictionary<string, string> ResolvedAddresses = new();

    public string Resolve(string address)
    {
        return ResolvedAddresses.GetOrAdd(address, ResolveAddress);
    }

    private static string ResolveAddress(string address)
    {
        var (host, port) = SplitHostAndPort(address);

        var addresses = Dns.GetHostAddresses(host);
        var resolved = addresses.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork)
                       ?? addresses.FirstOrDefault()
                       ?? throw new InvalidOperationException($"Could not resolve any IP address for DNS '{host}'.");

        return $"{resolved}:{port}";
    }

    private static (string Host, int Port) SplitHostAndPort(string value)
    {
        var segments = value.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        var host = segments[0];
        var port = segments.Length > 1 && int.TryParse(segments[1], out var parsedPort) ? parsedPort : DefaultServerPort;

        return (host, port);
    }
}