using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.Sdk.DependencyInjection;

public static class AppInjection
{
    private static readonly Assembly[] Assemblies = [typeof(AppInjection).Assembly];

    public static void AddPlayStatsSdk(this IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(scan => scan
            .FromAssemblies(Assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => Assemblies.Contains(type.Assembly)));

        serviceCollection.AddScoped(r => r.GetRequiredService<IPlayStatsContext>().RankingService);
        serviceCollection.AddScoped(r => r.GetRequiredService<IPlayStatsContext>().MatchesService);
        serviceCollection.AddScoped(r => r.GetRequiredService<IPlayStatsContext>().StatisticsService);
    }
}