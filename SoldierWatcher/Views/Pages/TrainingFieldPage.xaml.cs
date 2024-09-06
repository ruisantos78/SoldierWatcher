using CommunityToolkit.Mvvm.DependencyInjection;
using SoldierWatcher.ViewModels;
using System.Windows.Controls;

namespace SoldierWatcher.Views.Pages;

public partial class TrainingFieldPage : Page
{
    public TrainingFieldPage()
    {
        InitializeComponent();

        DataContext = Ioc.Default.GetRequiredService<TrainingFieldViewModel>();
    }
}
