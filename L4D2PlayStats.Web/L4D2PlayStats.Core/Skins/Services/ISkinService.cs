namespace L4D2PlayStats.Core.Skins.Services;

public interface ISkinService
{
    IReadOnlyCollection<Skin> GetAllSkins();
    IReadOnlyCollection<Skin> GetSkinsByLevel(int level);
}