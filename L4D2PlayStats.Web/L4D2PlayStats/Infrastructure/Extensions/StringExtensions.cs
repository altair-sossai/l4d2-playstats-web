namespace L4D2PlayStats.Infrastructure.Extensions;

public static class StringExtensions
{
    public static string Truncate(this string value, int max)
    {
        return value.Length <= max ? value : value[..max];
    }
}