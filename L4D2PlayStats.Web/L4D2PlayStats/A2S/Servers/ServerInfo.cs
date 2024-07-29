using L4D2PlayStats.A2S.Servers.Enums;
using L4D2PlayStats.Infrastructure.Extensions;

namespace L4D2PlayStats.A2S.Servers;

public class ServerInfo
{
    public ServerInfo(ref BinaryReader binReader)
    {
        Header = binReader.ReadByte();
        Protocol = binReader.ReadByte();
        Name = binReader.ReadNullTerminatedString();
        Map = binReader.ReadNullTerminatedString();
        Folder = binReader.ReadNullTerminatedString();
        Game = binReader.ReadNullTerminatedString();
        Id = binReader.ReadInt16();
        Players = binReader.ReadByte();
        MaxPlayers = binReader.ReadByte();
        Bots = binReader.ReadByte();
        ServerType = (ServerTypeFlags)binReader.ReadByte();
        Environment = (EnvironmentFlags)binReader.ReadByte();
        Visibility = (VisibilityFlags)binReader.ReadByte();
        Vac = (VacFlags)binReader.ReadByte();
        Version = binReader.ReadNullTerminatedString();
        ExtraDataFlag = (ExtraDataFlags)binReader.ReadByte();

        if (ExtraDataFlag.HasFlag(ExtraDataFlags.Port))
            Port = binReader.ReadInt16();

        if (ExtraDataFlag.HasFlag(ExtraDataFlags.SteamId))
            SteamId = binReader.ReadUInt64();

        if (ExtraDataFlag.HasFlag(ExtraDataFlags.Spectator))
        {
            SpectatorPort = binReader.ReadInt16();
            Spectator = binReader.ReadNullTerminatedString();
        }

        if (ExtraDataFlag.HasFlag(ExtraDataFlags.Keywords))
            Keywords = binReader.ReadNullTerminatedString();

        if (ExtraDataFlag.HasFlag(ExtraDataFlags.GameId))
            GameId = binReader.ReadUInt64();
    }

    public byte Header { get; set; }
    public byte Protocol { get; set; }
    public string? Name { get; set; }
    public string? Map { get; set; }
    public string? Folder { get; set; }
    public string? Game { get; set; }
    public short Id { get; set; }
    public byte Players { get; set; }
    public byte MaxPlayers { get; set; }
    public byte Bots { get; set; }
    public ServerTypeFlags ServerType { get; set; }
    public EnvironmentFlags Environment { get; set; }
    public VisibilityFlags Visibility { get; set; }
    public VacFlags Vac { get; set; }
    public string? Version { get; set; }
    public ExtraDataFlags ExtraDataFlag { get; set; }
    public ulong GameId { get; set; }
    public ulong SteamId { get; set; }
    public string? Keywords { get; set; }
    public string? Spectator { get; set; }
    public short SpectatorPort { get; set; }
    public short Port { get; set; }
}