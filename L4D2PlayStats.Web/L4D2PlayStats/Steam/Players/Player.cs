using L4D2PlayStats.Infrastructure.Extensions;

namespace L4D2PlayStats.Steam.Players;

public class Player(ref BinaryReader binReader)
{
    public byte Index { get; } = binReader.ReadByte();
    public string Name { get; } = binReader.ReadNullTerminatedString();
    public int Score { get; } = binReader.ReadInt32();
    public float Duration { get; } = binReader.ReadSingle();
    public TimeSpan DurationTimeSpan => TimeSpan.FromSeconds(Duration);
}