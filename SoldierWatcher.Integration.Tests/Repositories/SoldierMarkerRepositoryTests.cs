using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.MemoryCache;
using SoldierWatcher.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoldierWatcher.Integration.Tests.Repositories;

public class SoldierMarkerRepositoryTests
{
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private readonly IServiceProvider services;

    public SoldierMarkerRepositoryTests()
    {
        var builder = new ServiceCollection();
        builder.AddSoldierWatcherMemoryCache();
        services = builder.BuildServiceProvider();
    }

    [Fact(DisplayName = "Should get soldiers by training location async with success")]
    public async Task ShouldGetSoldiersByTrainingLocationAsyncWithSuccess()
    {
        // Arrange
        const string expectedSoldierName = "John Smith";
        
        var trainingLocationRepository = services.GetRequiredService<ITrainingLocationsRepository>();
        var trainingLocation = await trainingLocationRepository.GetTrainingLocationByIdAsync(Guid.Parse("e9b1c8f0-d10b-4c3e-9a16-5c6de6b6a684"), cancellationTokenSource.Token);
        trainingLocation.Should().NotBeNull();

        var repository = services.GetRequiredService<ISoldierMarkerRepository>();

        // Act
        var result = await repository.GetSoldiersByTrainingLocationAsync(trainingLocation!, cancellationTokenSource.Token);

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain(marker => marker.Soldier.Name == expectedSoldierName);
    }
}
