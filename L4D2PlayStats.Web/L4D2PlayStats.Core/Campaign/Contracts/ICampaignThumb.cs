namespace L4D2PlayStats.Core.Campaign.Contracts;

public interface ICampaignThumb
{
    string this[string? campaign] { get; }
}