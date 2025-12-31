using System.Text.Json;
using L4D2PlayStats.Sdk.Converters;
using L4D2PlayStats.Sdk.Handlers;
using L4D2PlayStats.Sdk.Matches;
using L4D2PlayStats.Sdk.Punishments;
using L4D2PlayStats.Sdk.Ranking;
using L4D2PlayStats.Sdk.Statistics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace L4D2PlayStats.Sdk.DependencyInjection;

public static class AppInjection
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

    static AppInjection()
    {
        Options.Converters.Add(new DateTimeConverter());
    }

    extension(IServiceCollection serviceCollection)
    {
        public void AddPlayStatsSdk(IConfiguration configuration)
        {
            var apiUrl = configuration.GetValue<string>("PlayStatsApiUrl")!;
            var serverId = configuration.GetValue<string>("ServerId")!;
            var secret = configuration.GetValue<string>("PlayStatsSecret")!;

            var apiUri = new Uri(apiUrl);

            serviceCollection.AddTransient(_ => new AuthHeaderHandler(serverId, secret));

            serviceCollection
                .AddApi<IRankingService>(apiUri)
                .AddApi<IMatchesService>(apiUri)
                .AddApi<IStatisticsService>(apiUri)
                .AddApi<IPunishmentsService>(apiUri);
        }

        private IServiceCollection AddApi<T>(Uri baseAddress)
            where T : class
        {
            serviceCollection
                .AddRefitClient<T>(Settings)
                .ConfigureHttpClient(c => c.BaseAddress = baseAddress)
                .AddHttpMessageHandler<AuthHeaderHandler>();

            return serviceCollection;
        }
    }
}