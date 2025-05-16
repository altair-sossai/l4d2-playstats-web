namespace L4D2PlayStats.Core.Campaign;

public interface ICampaignThumb
{
    string this[string? campaign] { get; }
}