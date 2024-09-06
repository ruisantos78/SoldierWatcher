using Microsoft.Extensions.DependencyInjection;
using SoldierWatcher.Data.MemoryCache.Core;
using SoldierWatcher.Data.MemoryCache.Repositories;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Data.MemoryCache;

public static class ApplicationExtensions
{
    public static IServiceCollection AddSoldierWatcherMemoryCache(this IServiceCollection services)
    {
        // Internal Services
        services.AddMemoryCache();
        services.AddScoped<IDataContext, DataContext>();

        // External Services
        services.AddScoped<ICountriesRepository, CountriesRepository>();
        services.AddScoped<ISensorsRepository, SensorsRepository>();
        services.AddScoped<ISoldiersRepository, SoldiersRepository>();
        services.AddScoped<ISoldierMarkerRepository, SoldierMarkerRepository>();
        services.AddScoped<ITrainingLocationsRepository, TrainingLocationsRepository>();

        return services;
    }
}
