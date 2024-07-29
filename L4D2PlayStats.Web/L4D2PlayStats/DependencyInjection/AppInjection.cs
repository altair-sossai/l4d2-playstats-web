using System.Reflection;
using L4D2PlayStats.Steam;
using Microsoft.Extensions.DependencyInjection;

namespace L4D2PlayStats.DependencyInjection;

public static class AppInjection
{
    public static void AddApp(this IServiceCollection serviceCollection)
    {
        var assemblies = new[]
        {
            Assembly.Load("L4D2PlayStats")
        };

        serviceCollection.AddMemoryCache();

        serviceCollection.Scan(scan => scan
            .FromAssemblies(assemblies)
            .AddClasses()
            .AsImplementedInterfaces(type => assemblies.Contains(type.Assembly)));

        serviceCollection.AddScoped(sp => sp.GetRequiredService<ISteamContext>().SteamUserService);
        serviceCollection.AddScoped(sp => sp.GetRequiredService<ISteamContext>().ServerInfoService);
    }
}