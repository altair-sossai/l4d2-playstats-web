namespace L4D2PlayStats.Campaign;

public interface ICampaignName
{
    string? this[string? map] { get; }
}