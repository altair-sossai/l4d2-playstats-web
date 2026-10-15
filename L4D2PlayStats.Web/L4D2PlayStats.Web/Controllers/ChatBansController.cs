using L4D2PlayStats.Core.ChatBans.Results;
using L4D2PlayStats.Core.ChatBans.Services;
using L4D2PlayStats.Web.Attributes;
using L4D2PlayStats.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Authorize]
[AdminAuthorize]
public class ChatBansController(IChatBanService chatBanService) : Controller
{
    private const int PageSize = 20;

    [Route("chat-bans")]
    public async Task<IActionResult> Index(string? q, int page = 1, CancellationToken cancellationToken = default)
    {
        ViewBag.Admin = "active";
        ViewBag.ChatBans = "active";

        var bans = await chatBanService.GetAllAsync(cancellationToken);
        var term = q?.Trim();

        var filtered = bans
            .Where(ban => string.IsNullOrEmpty(term)
                          || ban.Name.Contains(term, StringComparison.OrdinalIgnoreCase)
                          || ban.CommunityId.Contains(term, StringComparison.OrdinalIgnoreCase)
                          || (ban.SteamId?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false)
                          || (ban.Reason?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false))
            .ToList();

        var model = new ChatBansModel(new PagedListModel<ChatBanResult>(filtered, page, PageSize), q, bans.Count);

        return View(model);
    }
}
