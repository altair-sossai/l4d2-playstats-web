using System.Reflection;
using Stubble.Core.Builders;
using Stubble.Core.Interfaces;

namespace L4D2PlayStats.Ranking.Services;

public class RankingTemplateService : IRankingTemplateService
{
    private const string EmbeddedResourceName = "L4D2PlayStats.Ranking.Templates.RankingPlainText.txt";

    private static readonly Lazy<Type> Type = new(() => typeof(RankingTemplateService));
    private static readonly Lazy<Assembly> Assembly = new(() => Type.Value.Assembly);

    private static readonly Lazy<string> Template = new(() =>
    {
        using var stream = Assembly.Value.GetManifestResourceStream(EmbeddedResourceName);

        if (stream == null)
            throw new InvalidOperationException("Resource not found");

        using var streamReader = new StreamReader(stream);

        return streamReader.ReadToEnd();
    });

    private static readonly Lazy<IStubbleRenderer> Stubble = new(() => new StubbleBuilder().Build());

    public string Render<T>(T data)
    {
        return Stubble.Value.Render(Template.Value, data);
    }
}