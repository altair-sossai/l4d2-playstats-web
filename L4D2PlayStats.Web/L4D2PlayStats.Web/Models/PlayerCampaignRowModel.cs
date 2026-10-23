using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerCampaignRowModel(string name, PlayerCampaignResult? first, PlayerCampaignResult? second)
{
    public string Name { get; } = name;
    public PlayerCampaignResult? First { get; } = first;
    public PlayerCampaignResult? Second { get; } = second;
    public int Games => (First?.Games ?? 0) + (Second?.Games ?? 0);
}
