namespace L4D2PlayStats.Campaign;

public class CampaignThumb : ICampaignThumb
{
    private static readonly HashSet<string> Campaigns =
    [
        "Dead Center",
        "Dark Carnival",
        "Swamp Fever",
        "Hard Rain",
        "The Parish",
        "The Passing",
        "The Sacrifice",
        "No Mercy",
        "Crash Course",
        "Death Toll",
        "Dead Air",
        "Blood Harvest",
        "Cold Stream",
        "The Last Stand",
        "Dark Carnival Remix",
        "Open Road"
    ];

    public string this[string? campaign]
    {
        get
        {
            var campaignId = !string.IsNullOrEmpty(campaign) && Campaigns.Contains(campaign) ? campaign : "no-image";

            campaignId = campaignId.ToLower().Replace(" ", "-");

            return $"/imgs/campaigns/{campaignId}.jpg";
        }
    }
}