using CommunityToolkit.Mvvm.ComponentModel;

namespace SoldierWatcher.ViewModels;

public partial class MainViewModel: ObservableObject
{
    private Uri? _currentPage;

    public Uri? CurrentPage
    {
        get => _currentPage;
        set => SetProperty(ref _currentPage, value);
    }

    public MainViewModel() { 
        CurrentPage = new Uri("Pages/TrainingFieldPage.xaml", UriKind.Relative);
    }
}
