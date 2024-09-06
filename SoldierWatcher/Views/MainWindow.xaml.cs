using CommunityToolkit.Mvvm.DependencyInjection;
using SoldierWatcher.ViewModels;
using System.Windows;

namespace SoldierWatcher.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
    }
}
