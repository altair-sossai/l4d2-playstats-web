namespace L4D2PlayStats.Sdk.Ranking.Results;

public class PlayerCampaignResult
{
    public int Wins { get; set; }
    public int Loss { get; set; }
    public int Draws { get; set; }
    public int RageQuit { get; set; }
    public int Games => Wins + Loss + Draws;
    public decimal WinRate => Rate(Wins);
    public decimal LossRate => Rate(Loss);
    public decimal DrawRate => Rate(Draws);

    private decimal Rate(int value)
    {
        return Games == 0 ? 0 : value / (decimal)Games;
    }
}
