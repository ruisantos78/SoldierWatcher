using SoldierWatcher.Service.Client.EventHandlers;
using SoldierWatcher.Service.Client.Interfaces;
using SoldierWatcher.Service.Client.Services.Structs;
using System.Collections.Concurrent;

namespace SoldierWatcher.Service.Client.Services;

internal class GeolocationService : IGeolocationService
{
    public event EventHandler<GeolocationEventArgs>? GeolocationUpdated;

    private readonly ConcurrentDictionary<string, Coordinates> listeners = [];

    public Task ConnectAsync(string host, CancellationToken cancellationToken = default)
    {
        SimulateUpdatesServiceAsync(cancellationToken).ConfigureAwait(false);
        return Task.CompletedTask;
    }

    public void AddListener(string serialNumber)
    {
        if (string.IsNullOrEmpty(serialNumber))
            return;

        if (!listeners.ContainsKey(serialNumber))
            listeners.TryAdd(serialNumber, new Coordinates(38.7223, -9.1393));
    }

    public void RemoveListener(string serialNumber)
    {
        listeners.TryRemove(serialNumber, out _);
    }

    private async Task SimulateUpdatesServiceAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            foreach (var key in listeners.Keys)
            {
                if (!listeners.TryGetValue(key, out var coordinates))
                    continue;

                var newCoordinates = GenerateRandomCoordinates(coordinates);
                listeners.TryUpdate(key, newCoordinates, coordinates);
                OnGeolocationUpdated(key, newCoordinates);
            }

            await Task.Delay(500, cancellationToken);
        }
    }

    private void OnGeolocationUpdated(string serialNumber, Coordinates newCoordinates)
    {
        GeolocationUpdated?.Invoke(this, new GeolocationEventArgs(serialNumber, newCoordinates.Latitude, newCoordinates.Longitude));
    }


    private static Coordinates GenerateRandomCoordinates(Coordinates current)
    {
        var random = new Random();
        var latitude = current.Latitude + (random.NextDouble() - 0.5) / 10000;
        var longitude = current.Longitude + (random.NextDouble() - 0.5) / 10000;

        return new Coordinates(latitude, longitude);
    }
}
