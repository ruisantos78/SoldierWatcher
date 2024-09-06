using SoldierWatcher.Service.Client.EventHandlers;

namespace SoldierWatcher.Service.Client.Interfaces;

public interface IGeolocationService
{
    event EventHandler<GeolocationEventArgs> GeolocationUpdated;

    Task ConnectAsync(string host, CancellationToken cancellationToken = default);

    void AddListener(string serialNumber);
    void RemoveListener(string serialNumber);    
}