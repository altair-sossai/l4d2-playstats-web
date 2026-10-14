using System.Globalization;
using L4D2PlayStats.Core.Skins;

namespace L4D2PlayStats.Web.Models;

public class SkinLevelModel(Skin skin)
{
    public string Id => skin.Id;
    public string Weapon => skin.Weapon;
    public int Level => skin.Level;
    public bool Released => skin.IsReleased;
    public string ReleaseDate => skin.ReleaseDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
}