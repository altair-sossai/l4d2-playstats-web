using L4D2PlayStats.Sdk.Statistics.Enums;

namespace L4D2PlayStats.Sdk.Statistics.Extensions;

public static class PlayerStatsExtensions
{
    public static DataType DataType(this PlayerStats playerStats)
    {
        return playerStats switch
        {
            PlayerStats.TimeStartPresent
                or PlayerStats.TimeStopPresent
                or PlayerStats.TimeStartAlive
                or PlayerStats.TimeStopAlive
                or PlayerStats.TimeStartUpright
                or PlayerStats.TimeStopUpright
                => Enums.DataType.DateTime,
            _ => Enums.DataType.Number
        };
    }
}