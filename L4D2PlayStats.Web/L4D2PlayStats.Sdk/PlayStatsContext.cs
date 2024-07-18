using System.Text.Json;
using L4D2PlayStats.Sdk.Converters;
using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Ranking;
using Microsoft.Extensions.Configuration;
using Refit;

namespace L4D2PlayStats.Sdk;

public class PlayStatsContext(IConfiguration configuration)
    : IPlayStatsContext
{
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(Options)
    };

    private readonly string _baseUrl = configuration.GetValue<string>("PlayStatsApiUrl")!;

    static PlayStatsContext()
    {
        Options.Converters.Add(new DateTimeConverter());
    }

    public IRankingService RankingService => CreateService<IRankingService>();
    public IMatchesService MatchesService => CreateService<IMatchesService>();

    private T CreateService<T>()
    {
        return RestService.For<T>(_baseUrl, Settings);
    }
}