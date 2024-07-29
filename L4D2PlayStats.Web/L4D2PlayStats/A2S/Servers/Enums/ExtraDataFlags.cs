namespace L4D2PlayStats.A2S.Servers.Enums;

[Flags]
public enum ExtraDataFlags : byte
{
    GameId = 0x01,
    SteamId = 0x10,
    Keywords = 0x20,
    Spectator = 0x40,
    Port = 0x80
}