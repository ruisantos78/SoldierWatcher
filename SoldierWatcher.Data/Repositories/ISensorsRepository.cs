using SoldierWatcher.Data.Entities;

namespace SoldierWatcher.Data.Repositories;

public interface ISensorsRepository
{
    Task<Sensor?> GetSensorByIdAsync(Guid id, CancellationToken cancellationToken = default);
}
