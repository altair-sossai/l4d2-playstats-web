namespace L4D2PlayStats.Web.Models;

public class AntiCheatModel(string? version, string downloadUrl)
{
    public string? Version { get; init; } = version;
    public string DownloadUrl { get; init; } = downloadUrl;
}