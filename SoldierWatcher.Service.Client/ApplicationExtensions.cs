using Microsoft.Extensions.DependencyInjection;
using SoldierWatcher.Service.Client.Interfaces;
using SoldierWatcher.Service.Client.Services;

namespace SoldierWatcher.Service.Client;

public static class ApplicationExtensions
{
    public static IServiceCollection AddSoldierWatcherServices(this IServiceCollection services)
    {
        services.AddSingleton<IGeolocationService, GeolocationService>();

        return services;
    }
}
