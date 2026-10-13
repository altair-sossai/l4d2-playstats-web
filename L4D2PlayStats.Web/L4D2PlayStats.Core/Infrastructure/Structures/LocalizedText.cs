using System.Globalization;

namespace L4D2PlayStats.Core.Infrastructure.Structures;

public class LocalizedText
{
    public string En { get; init; } = string.Empty;
    public string Pt { get; init; } = string.Empty;
    public string Es { get; init; } = string.Empty;

    public string Get(CultureInfo culture)
    {
        var text = culture.TwoLetterISOLanguageName switch
        {
            "pt" => Pt,
            "es" => Es,
            _ => En
        };

        return string.IsNullOrWhiteSpace(text) ? En : text;
    }
}