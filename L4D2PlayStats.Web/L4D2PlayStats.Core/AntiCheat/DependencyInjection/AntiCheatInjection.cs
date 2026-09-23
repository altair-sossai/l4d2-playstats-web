using System.Text.Json;
using L4D2PlayStats.Core.AntiCheat.Services;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace L4D2PlayStats.Core.AntiCheat.DependencyInjection;

public static class AntiCheatInjection
{
    private static readonly RefitSettings Settings = new()
    {
        ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        })
    };

    extension(IServiceCollection serviceCollection)
    {
        public void AddAntiCheatServices()
        {
            serviceCollection
                .AddRefitGeneratedClient<IAntiCheatService>(Settings)
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(AppConsts.AntiCheatApiUrl);
                    c.Timeout = TimeSpan.FromSeconds(3);
                });
        }
    }
}