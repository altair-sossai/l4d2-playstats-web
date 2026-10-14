namespace L4D2PlayStats.Core.Infrastructure.Helpers;

public static class BrasiliaTime
{
    private static readonly TimeZoneInfo TimeZone = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

    public static DateTimeOffset StartOfDay(DateOnly date)
    {
        var local = date.ToDateTime(TimeOnly.MinValue);

        return new DateTimeOffset(local, TimeZone.GetUtcOffset(local));
    }
}