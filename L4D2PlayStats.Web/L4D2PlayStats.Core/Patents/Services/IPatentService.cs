using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Core.Patents.Services;

public interface IPatentService
{
    IReadOnlyCollection<Patent> GetAllPatents();
    Patent GetPatent(decimal experience);
    Patent? GetNextPatent(decimal experience);
    PatentProgress GetPatentProgress(PlayerResult player);
}