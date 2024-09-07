using FluentAssertions;
using SoldierWatcher.ViewModels;

namespace SoldierWatcher.Tests.ViewModels;

public class MainViewModelTests
{    
    [Fact(DisplayName = "Should Intialize Properties With Success")]
    public void ShouldIntializePropertiesWithSuccess()
    {
        // Arrange && Act
        var viewModel = new MainViewModel();

        // Assert
        viewModel.Should().NotBeNull();
        viewModel.CurrentPage.Should().BeEquivalentTo(new Uri("Pages/TrainingFieldPage.xaml", UriKind.Relative));
    }
}
