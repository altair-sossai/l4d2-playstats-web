using L4D2PlayStats.Web.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

[Authorize]
[AdminAuthorize]
public class ChatBansController : Controller
{
    [Route("chat-bans")]
    public IActionResult Index()
    {
        ViewBag.ChatBans = "active";

        return View();
    }
}