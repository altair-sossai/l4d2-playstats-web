namespace L4D2PlayStats.Sdk.Statistics.Extensions;

public static class DateTimeExtensions
{
    private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static long ToUnixTimeSeconds(this DateTime? dateTime)
    {
        return dateTime?.ToUnixTimeSeconds() ?? 0;
    }

    public static long ToUnixTimeSeconds(this DateTime dateTime)
    {
        var utcDateTime = dateTime.ToUniversalTime();
        var totalSeconds = (utcDateTime - UnixEpoch).TotalSeconds;

        return (long)totalSeconds;
    }
}