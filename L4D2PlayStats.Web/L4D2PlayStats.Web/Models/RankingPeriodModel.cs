namespace L4D2PlayStats.Web.Models;

public class RankingPeriodModel
{
    private DateTime _end;

    public RankingPeriodModel()
    {
        var now = DateTime.Now;

        Start = now.Month switch
        {
            1 or 2 => new DateTime(now.Year, 1, 1),
            3 or 4 => new DateTime(now.Year, 3, 1),
            5 or 6 => new DateTime(now.Year, 5, 1),
            7 or 8 => new DateTime(now.Year, 7, 1),
            9 or 10 => new DateTime(now.Year, 9, 1),
            _ => new DateTime(now.Year, 11, 1)
        };
    }

    private DateTime Start
    {
        set => _end = value.AddMonths(2).AddTicks(-1);
    }

    public int NextResetIn => (int)Math.Ceiling((_end - DateTime.Now).TotalDays);
}