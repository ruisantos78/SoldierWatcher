using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SoldierWatcher.Service.Client;
using SoldierWatcher.Service.Client.Interfaces;

namespace SoldierWatcher.Integration.Tests.Services;

public class GeolocationServiceTests
{
    private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
    private readonly IServiceProvider services;

    public GeolocationServiceTests()
    {
        var builder = new ServiceCollection();
        builder.AddSoldierWatcherServices();
        services = builder.BuildServiceProvider();  
    }

    [Fact(DisplayName = "Should connect with success")]
    public async Task ShouldConnectAsyncWithSuccess()
    {
        // Arrange
        var geolocationService = services.GetRequiredService<IGeolocationService>();

        // Act
        var action = () => geolocationService.ConnectAsync("fake://host", cancellationTokenSource.Token);

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact(DisplayName = "Should get listeners with success")]
    public async Task ShouldGetListenersAsyncWithSuccess()
    {
        // Arrange
        var geolocationService = services.GetRequiredService<IGeolocationService>();

        // Act
        var action = () => geolocationService.GetListenersAsync();

        // Assert
        await action.Should().NotThrowAsync();
    }

    [Fact(DisplayName = "Should add listener with success")]
    public async Task ShouldAddListenerAsyncWithSuccess()
    {
        // Arrange
        var serialNumber = Guid.NewGuid().ToString();

        var geolocationService = services.GetRequiredService<IGeolocationService>();

        var listeners = await geolocationService.GetListenersAsync();
        listeners.Should().NotContainKey(serialNumber);

        // Act
        var action = () => geolocationService.AddListenerAsync(serialNumber);

        // Assert
        await action.Should().NotThrowAsync();

        listeners = await geolocationService.GetListenersAsync();
        listeners.Should().ContainKey(serialNumber);

        // Cleanup
        await geolocationService.RemoveListenerAsync(serialNumber);
    }

    [Fact(DisplayName = "Should remove listener with success")]
    public async Task ShouldRemoveListnerAsyncWithSuccess()
    {
        // Arrange
        var serialNumber = Guid.NewGuid().ToString();

        var geolocationService = services.GetRequiredService<IGeolocationService>();

        await geolocationService.AddListenerAsync(serialNumber); ;

        // Act
        var action = () => geolocationService.RemoveListenerAsync(serialNumber);

        // Assert
        await action.Should().NotThrowAsync();

        var listeners = await geolocationService.GetListenersAsync();
        listeners.Should().NotContainKey(serialNumber);
    }
}
