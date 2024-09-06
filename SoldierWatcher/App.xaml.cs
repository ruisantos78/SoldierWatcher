using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SoldierWatcher.Data.MemoryCache;
using SoldierWatcher.Service.Client;
using SoldierWatcher.ViewModels;
using SoldierWatcher.Views;
using System.Windows;

namespace SoldierWatcher;

public partial class App : Application
{
    private readonly CancellationTokenSource cancellationTokenSource = new();

    protected override void OnStartup(StartupEventArgs e)
    {
        Ioc.Default.ConfigureServices(ConfigureServices());

        MainWindow = new MainWindow();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e)
    {
        cancellationTokenSource.Cancel();
        base.OnExit(e);
    }

    private ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Using MemoryCache Repository
        services.AddSoldierWatcherMemoryCache();
        services.AddSoldierWatcherServices();
        
        // View Models
        services.AddSingleton<MainViewModel>();
        services.AddSingleton<TrainingFieldViewModel>();

        // Other Services
        services.AddSingleton(cancellationTokenSource);

        return services.BuildServiceProvider();
    }
}
