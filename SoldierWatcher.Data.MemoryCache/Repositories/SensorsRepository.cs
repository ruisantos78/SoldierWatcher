using SoldierWatcher.Data.Entities;
using SoldierWatcher.Data.MemoryCache.Core;
using SoldierWatcher.Data.MemoryCache.Entities;
using SoldierWatcher.Data.Repositories;

namespace SoldierWatcher.Data.MemoryCache.Repositories;

internal class SensorsRepository: ISensorsRepository
{
    private readonly IDataContext dataContext;

    public SensorsRepository(IDataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    public async Task<Sensor?> GetSensorByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var sensors = await GetSensorsEnititesAsync(cancellationToken);

        if (sensors.FirstOrDefault(x => x.Id == id) is { } sensor)
            return ToSensor(sensor);

        return null;
    }

    private Task<SensorEntity[]> GetSensorsEnititesAsync(CancellationToken cancellationToken)
    {
        return dataContext.GetEntitiesAsync<SensorEntity>("Sensors", cancellationToken);
    }

    private static Sensor ToSensor(SensorEntity entity) => new()
    {
        Id = entity.Id,
        SensorName = entity.SensorName,
        SensorType = entity.SensorType
    };
}
