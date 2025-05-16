namespace L4D2PlayStats.Core.Campaign;

public interface ICampaignName
{
    string? this[string? map] { get; }
}