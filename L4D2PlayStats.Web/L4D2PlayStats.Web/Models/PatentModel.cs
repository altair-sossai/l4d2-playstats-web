using L4D2PlayStats.Core.Patents;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PatentModel(ExperienceConfigResult config, IReadOnlyCollection<Patent> patents)
{
    public ExperienceConfigResult Config { get; init; } = config;
    public IReadOnlyCollection<Patent> Patents { get; init; } = patents;
}