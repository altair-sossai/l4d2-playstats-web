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
        "Open Road",
        "Arena of the Dead 2",
        "Back to School",
        "Carried Off",
        "Dam It! The Director's Cut",
        "Dark Blood 2",
        "Dead Before Dawn DC",
        "Detour Ahead",
        "Diescraper Redux",
        "Firetower Trail",
        "Hard Rain: Downpour",
        "Haunted Forest",
        "Heaven Can Wait",
        "I Hate Mountains 2",
        "Suicide Blitz 2",
        "The Bloody Moors",
        "The Undead Zone",
        "Urban Flight",
        "Warcelona",
        "Yama"
    ];

    public string this[string? campaign]
    {
        get
        {
            var campaignId = !string.IsNullOrEmpty(campaign) && Campaigns.Contains(campaign) ? campaign : "no-image";

            campaignId = campaignId.ToLower().Replace(" ", "-");
            campaignId = string.Join(string.Empty, campaignId.Where(c => c is >= 'a' and <= 'z' or >= '0' and <= '9' or '-'));

            return $"/imgs/campaigns/{campaignId}.jpg";
        }
    }
}