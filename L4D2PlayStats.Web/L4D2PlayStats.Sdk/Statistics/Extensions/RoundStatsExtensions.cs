using L4D2PlayStats.Sdk.Statistics.Enums;

namespace L4D2PlayStats.Sdk.Statistics.Extensions;

public static class RoundStatsExtensions
{
    public static DataType DataType(this RoundStats roundStats)
    {
        return roundStats switch
        {
            RoundStats.StartTime
                or RoundStats.EndTime
                or RoundStats.StartTimePause
                or RoundStats.StopTimePause
                or RoundStats.StartTimeTank
                or RoundStats.StopTimeTank
                => Enums.DataType.DateTime,
            _ => Enums.DataType.Number
        };
    }
}