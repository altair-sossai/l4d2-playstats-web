using L4D2PlayStats.Sdk.Statistics.Enums;

namespace L4D2PlayStats.Sdk.Statistics.Extensions;

public static class InfectedStatsExtensions
{
    public static DataType DataType(this InfectedStats infectedStats)
    {
        return infectedStats switch
        {
            InfectedStats.TimeStartPresent
                or InfectedStats.TimeStopPresent
                => Enums.DataType.DateTime,
            _ => Enums.DataType.Number
        };
    }
}