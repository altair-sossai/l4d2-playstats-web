namespace L4D2PlayStats.Web.Models.OrderBy.Enum
{
    public enum OrderByFilter
    {
        Experience,
        Wins,
        Loss,
        Si,
        Ci,
        WinRate,
        RageQuit
    }

    public static class OrderByFilterExtensions
    {
        public static string ToFriendlyString(this OrderByFilter orderBy)
        {
            return orderBy switch
            {
                OrderByFilter.Experience => "Experience",
                OrderByFilter.Wins => "Wins",
                OrderByFilter.Loss => "Loss",
                OrderByFilter.Si => "MVP SI",
                OrderByFilter.Ci => "MVP CI",
                OrderByFilter.WinRate => "Win Rate",
                OrderByFilter.RageQuit => "Rage Quit",
                _ => orderBy.ToString()
            };
        }
    }
}
