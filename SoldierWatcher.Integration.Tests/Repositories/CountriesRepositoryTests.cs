using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoldierWatcher.Data.MemoryCache;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Integration.Tests.Repositories;

public class CountriesRepositoryTests
{
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private readonly IServiceProvider services;

    public CountriesRepositoryTests()
    {
        var builder = new ServiceCollection();
        builder.AddSoldierWatcherMemoryCache();
        services = builder.BuildServiceProvider();
    }

    [Theory(DisplayName = "Should get all countries async with success")]
    [InlineData("d3d2e0b1-6d8e-4a2e-9fd0-3f9645f9e0b2", "United States")]
    [InlineData("b32e1f2e-6b44-4b42-8d2e-89d9a5d3675a", "United Kingdom")]
    public async Task ShouldGetCountryByIdAsyncWithSuccess(string id, string name)
    {
        // Arrange
        var repository = services.GetRequiredService<ICountriesRepository>();

        // Act
        var result = await repository.GetCountryByIdAsync(Guid.Parse(id), cancellationTokenSource.Token);

        // Assert
        result.Should().NotBeNull();
        result?.Name.Should().Be(name);
    }

    [Fact(DisplayName = "Should get country by id async returns null with not found")]
    public async Task ShouldGetCountryByIdAsyncReturnsNullWithNotFound()
    {
        // Arrange
        var repository = services.GetRequiredService<ICountriesRepository>();

        // Act
        var result = await repository.GetCountryByIdAsync(Guid.Empty, cancellationTokenSource.Token);

        // Assert
        result.Should().BeNull();
    }
}
