using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace L4D2PlayStats.Web.Extensions;

public static class GameEventHtmlExtensions
{
    public static IHtmlContent EventPlayer(this IHtmlHelper html, Core.GameInfo.Models.Player? player, string fallback)
    {
        var name = player?.Name ?? fallback;
        var url = player?.ProfileUrl;

        var tag = new TagBuilder(string.IsNullOrEmpty(url) ? "span" : "a");
        tag.AddCssClass("game-event-player");

        if (!string.IsNullOrEmpty(url))
        {
            tag.Attributes["href"] = url;
            tag.Attributes["target"] = "_blank";
            tag.Attributes["rel"] = "noopener noreferrer";
        }

        tag.InnerHtml.Append(name);

        return tag;
    }
}