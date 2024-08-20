using L4D2PlayStats.Patents.Services;
using L4D2PlayStats.Ranking;
using L4D2PlayStats.Sdk.Ranking.Results;
using L4D2PlayStats.UserAvatar;
using L4D2PlayStats.Web.Models;
using L4D2PlayStats.Web.Models.OrderBy;
using L4D2PlayStats.Web.Models.OrderBy.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace L4D2PlayStats.Web.Controllers;

public class HomeController(
    IStringLocalizer<SharedResource> sharedLocalizer,
    IRankingServiceCached rankingService,
    IUserAvatar userAvatar,
    IPatentService patentService) : Controller
{
    public async Task<IActionResult> Index(OrderByFilter orderBy)
    {
        ViewBag.Home = "active";
        ViewData["SelectedValue"] = OrderByFilterExtensions.ToFriendlyString(orderBy);
        var selectedOption = ViewData["SelectedValue"]!.ToString();


        var orderByOptions = Enum.GetValues(typeof(OrderByFilter))
                                  .Cast<OrderByFilter>()
                                  .Select(option => new OrderByOption
                                  {
                                      Value = option,
                                      FriendlyName = option.ToFriendlyString()
                                  })
                                  .ToList();

        OrderByOption optionToRemove = new OrderByOption();
        foreach (var option in orderByOptions)
        {
            if (option.FriendlyName!.Equals(selectedOption))
            {
                optionToRemove = option;
                break;
            }                
        }

        orderByOptions.Remove(optionToRemove);

        ViewBag.OrderByOptions = orderByOptions;

        var players = await rankingService.GetAsync();

        switch (orderBy)
        {
            case OrderByFilter.Wins:
                players = players.OrderByDescending(p => p.Wins).ToList();
                break;

            case OrderByFilter.Loss:
                players = players.OrderByDescending(p => p.Loss).ToList();
                break;

            case OrderByFilter.Si:
                players = players.OrderByDescending(p => p.MvpSiDamage).ToList();
                break;

            case OrderByFilter.Ci:
                players = players.OrderByDescending(p => p.MvpCommon).ToList();
                break;

            case OrderByFilter.WinRate:
                players = players.OrderByDescending(p => p.WinRate).ToList();
                break;

            case OrderByFilter.RageQuit:
                players = players.OrderByDescending(p => p.RageQuit).ToList();
                break;

            default:
                players = players.OrderByDescending(p => p.Experience).ToList();
                break;
        }

        var communityIds = players.Select(p => p.CommunityId);
        await userAvatar.LoadAsync(communityIds);

        var models = players.Select(player =>
        {
            var patentProgress = patentService.GetPatentProgress(player);
            var model = new RankingModel(sharedLocalizer, player, patentProgress);

            return model;
        }).ToList();

        return View(models);
    }

    public IActionResult SetTheme(string theme)
    {
        var option = new CookieOptions
        {
            Expires = DateTime.Now.AddYears(1)
        };

        Response.Cookies.Append("theme", theme, option);

        return Redirect(Request.Headers["Referer"].ToString());
    }
}