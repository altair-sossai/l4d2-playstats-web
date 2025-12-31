namespace L4D2PlayStats.Core.Campaign.Contracts;

public interface ICampaignName
{
    string? this[string? map] { get; }
}