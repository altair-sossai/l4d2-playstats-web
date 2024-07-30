using System.Net;
using System.Net.Sockets;
using System.Text;

namespace L4D2PlayStats.Steam.Players.Services;

public class PlayerService : IPlayerService
{
    private static readonly byte[] Request = [0xFF, 0xFF, 0xFF, 0xFF, 0x55, 0xFF, 0xFF, 0xFF, 0xFF];

    public async IAsyncEnumerable<Player> GetPlayersAsync(string ip, int port)
    {
        var ipAddress = IPAddress.Parse(ip);
        var ipEndPoint = new IPEndPoint(ipAddress, port);

        using var udpClient = new UdpClient();
        udpClient.Client.SendTimeout = 5000;
        udpClient.Client.ReceiveTimeout = 5000;

        await udpClient.SendAsync(Request, Request.Length, ipEndPoint);

        var result = await udpClient.ReceiveAsync();
        var bytes = result.Buffer;
        if (bytes.Length != 9 || bytes[4] != 0x41)
            yield break;

        bytes[4] = 0x55;
        await udpClient.SendAsync(bytes, bytes.Length, ipEndPoint);

        result = await udpClient.ReceiveAsync();

        using var memoryStream = new MemoryStream(result.Buffer);
        var binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);
        memoryStream.Seek(4, SeekOrigin.Begin);
        binaryReader.ReadByte();

        var count = binaryReader.ReadByte();

        for (var i = 0; i < count; i++)
        {
            var player = new Player(ref binaryReader);

            if (string.IsNullOrEmpty(player.Name))
                continue;

            yield return player;
        }

        udpClient.Close();
    }
}