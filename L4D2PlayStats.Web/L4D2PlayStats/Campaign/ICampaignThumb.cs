namespace L4D2PlayStats.Campaign;

public interface ICampaignThumb
{
    string this[string? campaign] { get; }
}