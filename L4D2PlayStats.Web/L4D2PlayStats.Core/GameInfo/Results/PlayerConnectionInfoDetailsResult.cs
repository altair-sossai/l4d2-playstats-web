namespace L4D2PlayStats.Core.GameInfo.Results;

public class PlayerConnectionInfoDetailsResult
{
    public required PlayerConnectionInfoResult? Player { get; set; }
    public required List<PlayerConnectionInfoResult> RelatedPlayers { get; set; }
}