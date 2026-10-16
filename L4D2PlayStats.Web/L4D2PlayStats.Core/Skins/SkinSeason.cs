namespace L4D2PlayStats.Core.Skins;

public class SkinSeason
{
    private SkinSeason(DateTimeOffset start)
    {
        Start = start;
        End = start.AddMonths(2);
    }

    public DateTimeOffset Start { get; }
    public DateTimeOffset End { get; }
    public DateTimeOffset LastDay => End.AddDays(-1);
    public int DaysLeft => Math.Max(0, (int)Math.Floor((End - DateTimeOffset.UtcNow).TotalDays));

    public static SkinSeason Current => Of(DateTimeOffset.UtcNow);

    public static SkinSeason Of(DateTimeOffset reference)
    {
        var utc = reference.ToUniversalTime();
        var month = utc.Month - (utc.Month - 1) % 2;

        return new SkinSeason(new DateTimeOffset(utc.Year, month, 1, 0, 0, 0, TimeSpan.Zero));
    }
}