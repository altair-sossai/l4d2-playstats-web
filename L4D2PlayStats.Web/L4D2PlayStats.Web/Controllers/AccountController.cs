using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace L4D2PlayStats.Web.Controllers;

public class AccountController : Controller
{
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        return Challenge(new AuthenticationProperties { RedirectUri = RedirectUrl(returnUrl) }, SteamAuthenticationDefaults.AuthenticationScheme);
    }

    [HttpGet("logout")]
    public IActionResult Logout(string? returnUrl = null)
    {
        return SignOut(new AuthenticationProperties { RedirectUri = RedirectUrl(returnUrl) }, CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private string RedirectUrl(string? returnUrl)
    {
        if (!string.IsNullOrEmpty(returnUrl))
            return returnUrl;

        var headers = HttpContext.Request.Headers;
        var referer = headers.Referer;

        return string.IsNullOrEmpty(referer) ? "/" : referer.ToString();
    }
}