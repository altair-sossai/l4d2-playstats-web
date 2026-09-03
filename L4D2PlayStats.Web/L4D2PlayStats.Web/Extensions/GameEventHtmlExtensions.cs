using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace L4D2PlayStats.Web.Extensions;

public static partial class GameEventHtmlExtensions
{
    [GeneratedRegex(@"\{(\d+)\}")]
    private static partial Regex PlaceholderRegex();

    extension(IHtmlHelper html)
    {
        public IHtmlContent EventPlayer(Core.GameInfo.Models.Player player)
        {
            return html.EventPlayer(player, string.Empty);
        }

        public IHtmlContent EventPlayer(Core.GameInfo.Models.Player? player, string fallback)
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

        public IHtmlContent EventSpan(string cssClass, string text)
        {
            var span = new TagBuilder("span");
            span.AddCssClass(cssClass);
            span.InnerHtml.Append(text);

            return span;
        }

        public IHtmlContent EventStat(string format, params object[] values)
        {
            var args = values
                .Select(value => html.EventSpan("game-event-highlight", Convert.ToString(value, CultureInfo.CurrentCulture) ?? string.Empty))
                .ToArray();

            return html.EventFormat(format, args);
        }

        public IHtmlContent EventFormat(string format, params IHtmlContent[] args)
        {
            var content = new HtmlContentBuilder();
            var regex = PlaceholderRegex();
            var segments = regex.Split(format);

            for (var i = 0; i < segments.Length; i++)
                if (i % 2 == 0)
                    content.Append(segments[i]);
                else
                    content.AppendHtml(args[int.Parse(segments[i], CultureInfo.InvariantCulture)]);

            return content;
        }
    }
}