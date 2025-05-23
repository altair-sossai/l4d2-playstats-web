using System.Text.Json;
using L4D2PlayStats.Core.Steam.ServerInfo.Services;
using L4D2PlayStats.Core.Steam.SteamUser.Services;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Retry;
using Refit;

namespace L4D2PlayStats.Core.Steam.DependencyInjection;

public static class SteamInjection
{
    private const string BaseUrl = "https://api.steampowered.com";

    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };

    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(Options)
    };

    private static readonly RetryStrategyOptions<HttpResponseMessage> RetryStrategyOptions = new()
    {
        MaxRetryAttempts = 3,
        Delay = TimeSpan.FromSeconds(2),
        BackoffType = DelayBackoffType.Exponential
    };

    public static void AddSteamServices(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSteamService<ISteamUserService>(nameof(ISteamUserService))
            .AddSteamService<IServerInfoService>(nameof(IServerInfoService));
    }

    private static IServiceCollection AddSteamService<T>(this IServiceCollection serviceCollection, string name)
        where T : class
    {
        serviceCollection
            .AddRefitClient<T>(Settings)
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(BaseUrl))
            .AddResilienceHandler(name, c => c.AddRetry(RetryStrategyOptions));

        return serviceCollection;
    }
}