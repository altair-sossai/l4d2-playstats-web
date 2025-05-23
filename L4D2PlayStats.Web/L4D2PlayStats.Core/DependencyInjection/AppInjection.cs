using L4D2PlayStats.Core.Steam.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.Core.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection serviceCollection)
    {
        var assemblies = new[]
        {
            typeof(AppInjection).Assembly
        };

        serviceCollection.AddMemoryCache();

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly)));

        serviceCollection.AddSteamServices();
    }
}