using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class AccountController : Controller
{
    [HttpGet("login")]
    public IActionResult Login()
    {
        return Challenge(new AuthenticationProperties { RedirectUri = "/" }, SteamAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        return SignOut(new AuthenticationProperties { RedirectUri = "/" }, CookieAuthenticationDefaults.AuthenticationScheme);
    }
}