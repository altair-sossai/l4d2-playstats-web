using L4D2PlayStats.Patents;
using L4D2PlayStats.Sdk.Ranking.Results;

namespace L4D2PlayStats.Web.Models;

public class PlayerPatentModel(PlayerResult player, PatentProgress patentProgress)
{
    public int Position => player.Position;
    public string CommunityId => player.CommunityId.ToString();
    public int Level => patentProgress.Patent.Level;
}