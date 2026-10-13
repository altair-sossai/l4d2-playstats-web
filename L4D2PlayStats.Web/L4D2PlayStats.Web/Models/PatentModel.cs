using L4D2PlayStats.Core.Patents;
using L4D2PlayStats.Core.Skins;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PatentModel(ExperienceConfigResult config, IReadOnlyCollection<Patent> patents, IReadOnlyCollection<Skin> skins)
{
    public ExperienceConfigResult Config { get; init; } = config;
    public IReadOnlyCollection<Patent> Patents { get; init; } = patents;
    public IReadOnlyCollection<Skin> Skins { get; init; } = skins;

    public IReadOnlyCollection<Skin> SkinsOf(int level)
    {
        return
        [
            .. Skins
                .Where(skin => skin.Level == level)
                .OrderBy(skin => Array.IndexOf(SkinsModel.Weapons, skin.Weapon))
                .ThenBy(skin => skin.Title.En)
        ];
    }
}