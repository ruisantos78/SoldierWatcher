using FluentAssertions;
using NSubstitute;
using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.Repositories;
using SoldierWatcher.Models;
using SoldierWatcher.Service.Client.EventHandlers;
using SoldierWatcher.Service.Client.Interfaces;
using SoldierWatcher.ViewModels;

namespace SoldierWatcher.Tests.ViewModels;

public class TrainingFieldViewModelTests
{
    private readonly CancellationTokenSource cancellationTokenSource = new();
    private readonly ITrainingLocationsRepository trainingLocationsRepository = Substitute.For<ITrainingLocationsRepository>();
    private readonly ISoldierMarkerRepository soldierMarkerRepository = Substitute.For<ISoldierMarkerRepository>();
    private readonly IGeolocationService geolocationService = Substitute.For<IGeolocationService>();


    [Fact(DisplayName = "Should Initialize Properties With Success")]
    public void ShouldInitializePropertiesWithSuccess()
    {
        // Arrange
        IReadOnlyList<TrainingLocation> trainingLocations = [new() { LocationName = "Location 1" }];
        IReadOnlyList<SoldierMarker> soldierMarkers = [new() { TrainingLocation = trainingLocations[0] }];
        IReadOnlyList<SoldierMarkerModel> expectedSoldiers = [new SoldierMarkerModel(soldierMarkers[0])];

        trainingLocationsRepository.GetTrainingLocationsAsync(cancellationTokenSource.Token)
            .ReturnsForAnyArgs(trainingLocations);

        soldierMarkerRepository.GetSoldiersByTrainingLocationAsync(Arg.Any<TrainingLocation>(), cancellationTokenSource.Token)
            .ReturnsForAnyArgs(soldierMarkers);

        // Act
        var viewModel = new TrainingFieldViewModel(cancellationTokenSource, trainingLocationsRepository, soldierMarkerRepository, geolocationService);
        Thread.Sleep(100); // Wait for async operations to complete

        // Assert
        viewModel.TrainingLocations.Should().BeEquivalentTo(trainingLocations);
        viewModel.SelectedTrainingLocation.Should().Be(trainingLocations[0]);
        viewModel.Soldiers.Should().BeEquivalentTo(expectedSoldiers, options => options.Excluding(x => x.LastUpdate));
        viewModel.SelectedSoldier.Should().BeNull();
    }

    [Fact(DisplayName = "Should Connect To Geolocation Service With Success")]
    public void ShouldConnectToGeolocationServiceWithSuccess()
    {
        // Arrange & Act
        _ = new TrainingFieldViewModel(cancellationTokenSource, trainingLocationsRepository, soldierMarkerRepository, geolocationService);
        Thread.Sleep(100); // Wait for async operations to complete

        // Assert
        geolocationService.Received(1).ConnectAsync(Arg.Any<string>(), cancellationTokenSource.Token);
    }

    [Fact(DisplayName = "Should Add Geolacation Listener For All Soldier With Success")]
    public void ShouldAddGeolacationListenerForAllSoldierWithSuccess()
    {
        // Arrange 
        string expectedSerialNumber = "1111-1111";

        trainingLocationsRepository.GetTrainingLocationsAsync(cancellationTokenSource.Token)
            .ReturnsForAnyArgs([new()]);

        soldierMarkerRepository.GetSoldiersByTrainingLocationAsync(Arg.Any<TrainingLocation>(), cancellationTokenSource.Token)
            .ReturnsForAnyArgs([new() { SerialNumber = expectedSerialNumber }]);

        // Act
        _ = new TrainingFieldViewModel(cancellationTokenSource, trainingLocationsRepository, soldierMarkerRepository, geolocationService);
        Thread.Sleep(100); // Wait for async operations to complete

        // Assert
        geolocationService.Received(1).AddListenerAsync(Arg.Is(expectedSerialNumber));
    }

    [Fact(DisplayName = "Should Update Soldier Coordenates")]
    public void ShouldGeolocationUpdateSoldierCoordenates()
    {
        // Arrange
        trainingLocationsRepository.GetTrainingLocationsAsync(cancellationTokenSource.Token)
            .ReturnsForAnyArgs([new()]);

        soldierMarkerRepository.GetSoldiersByTrainingLocationAsync(Arg.Any<TrainingLocation>(), cancellationTokenSource.Token)
            .ReturnsForAnyArgs([new() { SerialNumber = "1111-1111" }]);

        geolocationService.When(x => x.AddListenerAsync(Arg.Any<string>()))
            .Do(x => geolocationService.GeolocationUpdated += Raise.EventWith(this, new GeolocationEventArgs("1111-1111", 1.111, 2.222)));

        // Act
        var viewModel = new TrainingFieldViewModel(cancellationTokenSource, trainingLocationsRepository, soldierMarkerRepository, geolocationService);
        Thread.Sleep(100); // Wait for async operations to complete
            
        // Assert
        viewModel.Soldiers.Should().ContainEquivalentOf(new SoldierMarkerModel(new() { 
            SerialNumber = "1111-1111", 
            Latitude = 1.111, 
            Longitude = 2.222 }
        ), options => options.Excluding(x => x.LastUpdate));

        soldierMarkerRepository.Received(1).UpdateSoldierLocationAsync(Arg.Any<SoldierMarker>(), cancellationTokenSource.Token);
    }
}
