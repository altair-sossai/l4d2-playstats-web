using L4D2PlayStats.Core.AntiCheatAccess;
using L4D2PlayStats.Core.AntiCheatAccess.Enums;
using L4D2PlayStats.Core.AntiCheatAccess.Results;

namespace L4D2PlayStats.Web.Models;

public class AntiCheatAdminModel(AntiCheatAccessResult access, AntiCheatAccessList shownList, PagedListModel<AntiCheatAccessPlayerResult> players, string? query)
{
    public AntiCheatAccessResult Access { get; } = access;
    public AntiCheatAccessMode Mode => Access.Mode;
    public AntiCheatAccessList ActiveList => Access.Mode.ListOf();
    public AntiCheatAccessList ShownList { get; } = shownList;
    public AntiCheatAccessList OtherList => ShownList == AntiCheatAccessList.Whitelist ? AntiCheatAccessList.Required : AntiCheatAccessList.Whitelist;
    public bool ShowingActiveList => ShownList == ActiveList;
    public PagedListModel<AntiCheatAccessPlayerResult> Players { get; } = players;
    public string? Query { get; } = query;
    public int ShownCount => Access[ShownList].Count;
    public int OtherCount => Access[OtherList].Count;
    public string ShownListName => ShownList.ToName();
    public string OtherListName => OtherList.ToName();
}
