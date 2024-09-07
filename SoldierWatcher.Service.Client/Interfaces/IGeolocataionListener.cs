using SoldierWatcher.Service.Client.Entities;
using SoldierWatcher.Service.Client.EventHandlers;

namespace SoldierWatcher.Service.Client.Interfaces;

public interface IGeolocationService
{
    event EventHandler<GeolocationEventArgs> GeolocationUpdated;

    Task ConnectAsync(string host, CancellationToken cancellationToken = default);

    Task AddListenerAsync(string serialNumber);
    Task RemoveListenerAsync(string serialNumber);
    Task<IReadOnlyDictionary<string, Coordinates>> GetListenersAsync();
}