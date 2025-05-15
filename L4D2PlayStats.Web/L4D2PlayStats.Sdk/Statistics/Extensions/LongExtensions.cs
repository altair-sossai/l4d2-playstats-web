namespace L4D2PlayStats.Sdk.Statistics.Extensions;

public static class LongExtensions
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static DateTime? ToDateTime(this long value)
    {
        if (value <= 0)
            return null;

        return UnixEpoch.AddMilliseconds(value * 1000);
    }
}